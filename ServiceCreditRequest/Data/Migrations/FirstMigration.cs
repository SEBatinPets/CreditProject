using FluentMigrator;

namespace ServiceCreditRequest.Data.Migrations
{
    [Migration(1)]
    public class FirstMigration : Migration
    {
        public override void Up()
        {
            Create.Table("credit_requests")
                .WithColumn("id").AsInt32().Identity()
                .WithColumn("application_num").AsString()
                .WithColumn("application_date").AsDateTime()
                .WithColumn("branch_bank").AsString()
                .WithColumn("branch_bank_addr").AsString()
                .WithColumn("credit_manager_id").AsInt64()
                .WithColumn("scoring_status").AsBoolean().Nullable()
                .WithColumn("scoring_date").AsDateTime().Nullable();

            Create.Table("credit_applicants")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("first_name").AsString()
                .WithColumn("middle_name").AsString()
                .WithColumn("last_name").AsString()
                .WithColumn("date_birth").AsDateTime()
                .WithColumn("city_birth").AsString()
                .WithColumn("adress_birth").AsString()
                .WithColumn("adress_current").AsString()
                .WithColumn("inn").AsInt64()
                .WithColumn("snils").AsInt64()
                .WithColumn("passport_num").AsString();

            Create.Table("credit_contracts")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("credit_type").AsInt32()
                .WithColumn("requested_amount").AsInt64()
                .WithColumn("requested_currency").AsString()
                .WithColumn("annual_salary").AsInt64()
                .WithColumn("month_salary").AsInt64()
                .WithColumn("company_name").AsString()
                .WithColumn("comment").AsString();

            Create.Column("credit_applicant_id")
                .OnTable("credit_requests").AsInt32().ForeignKey("credit_applicants", "id");
            Create.Column("credit_contract_id")
                .OnTable("credit_requests").AsInt32().ForeignKey("credit_contracts", "id");
        }
        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}
