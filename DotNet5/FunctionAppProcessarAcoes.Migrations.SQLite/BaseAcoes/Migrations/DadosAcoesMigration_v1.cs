using FluentMigrator;

namespace BaseAcoes.Migrations
{
    [Migration(1)]
    public class DadosAcoesMigration_v1 : Migration
    {
        public override void Up()
        {
    		Create.Table("Acoes")
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey()
	    		.WithColumn("Codigo").AsString().NotNullable()
                .WithColumn("DataReferencia").AsDateTime().NotNullable()
	    		.WithColumn("Valor").AsDecimal().NotNullable()
                .WithColumn("CodCorretora").AsString().NotNullable()
                .WithColumn("NomeCorretora").AsString().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("Acoes");
        }
    }
}