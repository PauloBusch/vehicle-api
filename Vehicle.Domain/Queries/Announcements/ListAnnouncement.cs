using Dapper;
using Questor.Vehicle.Domain.Mutations.Vehicles.Entities.Enums;
using Questor.Vehicle.Domain.Queries.Announcements.ViewModels;
using Questor.Vehicle.Domain.Utils.Enums;
using Questor.Vehicle.Domain.Utils.Interfaces;
using Questor.Vehicle.Domain.Utils.Results;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Questor.Vehicle.Domain.Queries.Announcements
{
    public class ListAnnouncement : IQueryList<AnnouncementList>
    {
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 10;
        public string SortColumn { get; set; } = "date_creation";
        public EOrder SortOrder { get; set; } = EOrder.Desc;
        public int? Year { get; set; }
        public bool IncludeReserved { get; set; }
        public bool IncludeSold { get; set; }
        public DateTimeOffset? DateSale { get; set; }
        public EColor? ColorId { get; set; }
        public string BrandId { get; set; }
        public string ModelId { get; set; }

        private static string[] columnsSort = new [] { "id", "date_creation", "date_sale", "price_purchase", "price_sale", "year", "vehicle_model", "vehicle_brand" };
    
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
                from view_announcements_list a
                where 1=1
                    {(!IncludeReserved? $" and not exists(select r.id from reservations r where r.id_announcement=a.id)" : null)}
                    {(!IncludeSold && DateSale == null ? $" and a.date_sale is null" : null)}
                    {(DateSale != null ? $" and date_format(a.date_sale,'%Y-%m-%d')=@DataSale" : null)}
                    {(Year != null ? $" and a.vehicle_year=@Year" : null)}
                    {(ColorId != null ? $" and a.color_id=@ColorId" : null)}
                    {(BrandId != null ? $" and a.brand_id=@BrandId" : null)}
                    {(ModelId != null ? $" and a.model_id=@ModelId" : null)}
            ";

            var sqlResult = $@"
                -- paginate rows
                select
                    a.id, a.date_creation, a.date_sale, a.price_purchase, a.price_sale,
                    a.vehicle_year, a.vehicle_model_name, a.vehicle_brand_name, a.vehicle_fuel_name,
                    a.vehicle_color_name, a.vehicle_color_hex, a.photo_date
                {sqlBody}
                order by {sortColumnIndex} {sortOrderStr}
                limit @Offset, @Limit;
            
                -- count rows
                select count(a.id) as total {sqlBody};
            ";

            var parameters = new {
                Year,
                ColorId,
                BrandId,
                ModelId,
                DataSale = DateSale?.ToString("yyyy-MM-dd"),
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
