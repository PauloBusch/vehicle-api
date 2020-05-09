using Dapper;
using Questor.Vehicle.Domain.Queries.Announcements.ViewModels;
using Questor.Vehicle.Domain.Utils.Enums;
using Questor.Vehicle.Domain.Utils.Interfaces;
using Questor.Vehicle.Domain.Utils.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questor.Vehicle.Domain.Queries.Announcements
{
    public class ListAnnouncement : IQueryList<AnnouncementList>
    {
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 10;
        public string SortColumn { get; set; } = "id";
        public EOrder SortOrder { get; set; } = EOrder.Desc;
        public int? Year { get; set; }
        public bool OnlyUnsold { get; set; }
        public DateTime? DataSale { get; set; }
        public string BrandId { get; set; }
        public string ModelId { get; set; }

        private static string[] columnsSort = new [] { "id", "date_sale", "price_purchase", "price_sale", "year", "vehicle_model", "vehicle_brand" };
    
        public async Task<QueryResultList<AnnouncementList>> ValidateAsync(VehicleQueriesHandler handler)
        {
            if (Page <= 0) return new QueryResultList<AnnouncementList>(EStatusCode.InvalidData, $"Parameter {nameof(Page)} require postive");
            if (Limit <= 0) return new QueryResultList<AnnouncementList>(EStatusCode.InvalidData, $"Paramter {nameof(Limit)} require positive");
            if (!columnsSort.Contains(SortColumn)) return new QueryResultList<AnnouncementList>(EStatusCode.InvalidData, $"Parameter {nameof(SortColumn)} require in {string.Join(", ", columnsSort)}");

            return await Task.FromResult<QueryResultList<AnnouncementList>>(null);
        }

        public async Task<QueryResultList<AnnouncementList>> ExecuteAsync(VehicleQueriesHandler handler)
        {
            var sortColumnIndex = Array.IndexOf(columnsSort, SortColumn) + 1;
            var sortOrderStr = SortOrder == EOrder.Asc ? "asc" : "desc";
            var sqlBody = $@"
                from announcements a
                    join vehicles v on v.id=a.id_vehicle
                    join models m on m.id=v.id_model
                    join brands b on b.id=v.id_brand
                    join colors c on c.id=v.id_color
                where 1=1
                    {(OnlyUnsold ? $" and a.date_sale is null" : null)}
                    {(DataSale != null && !OnlyUnsold ? $" and date_format(a.date_sale,'%Y-%m-%d')=@DataSale" : null)}
                    {(Year != null ? $" and v.year=@Year" : null)}
                    {(BrandId != null ? $" and v.id_brand=@BrandId" : null)}
                    {(ModelId != null ? $" and v.id_model=@ModelId" : null)}
            ";

            var sqlResult = $@"
                -- paginate rows
                select
                    a.id, a.date_sale, a.price_purchase, a.price_sale,
                    v.year as vehicle_year, m.name as vehicle_model, b.name as vehicle_brand,
                    c.name as vehicle_color_name, c.hex as vehicle_color_hex
                {sqlBody}
                order by {sortColumnIndex} {sortOrderStr}
                limit @Offset, @Limit;
            
                -- count rows
                select count(a.id) as total {sqlBody};
            ";

            var parameters = new {
                Year,
                BrandId,
                ModelId,
                DataSale = DataSale?.ToString("yyyy-MM-dd"),
                Offset = (Page - 1) * Limit,
                Limit,
            };
            
            var data = await handler.DbConnection.QueryMultipleAsync(sqlResult, parameters);
            var announcements = await data.ReadAsync<AnnouncementList>();
            var totalRows = await data.ReadFirstAsync<Int64>();
            return new QueryResultList<AnnouncementList>(announcements, totalRows);
        }
    }
}
