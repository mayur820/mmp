namespace IRecordweb.Migrations
    {
    using System;
    using System.Data.Entity.Migrations;

    public partial class USP_Company : DbMigration
        {
        public override void Up()
            {
            CreateStoredProcedure(
                "dbo.USP_Company_Insert",
                p => new
                    {
                    Name = p.String(),
                    ReleaseDate = p.DateTime(),
                    Category = p.String(),
                    },
                body:
                    @"INSERT [dbo].[M_Company]([CountryId], [Code], [Name], [Description],[Address], [Pincode] , [StartDate] , [EndDate], [Active],[CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate] )  
                      VALUES (@CountryId, @Code,  @Name, @Description, @Address, @Pincode, getdate(), getdate(), @Active, @CreatedBy, getdate(), @ModifiedBy , getdate())  
                        
                      DECLARE @CountryId int  
                      SELECT @CountryId = [CountryId]  
                      FROM [dbo].[M_Company]  
                      WHERE @@ROWCOUNT > 0 AND [CountryId] = scope_identity()  
                        
                      SELECT t0.[]  
                      FROM [dbo].[M_Company] AS t0  
                      WHERE @@ROWCOUNT > 0 AND t0.[CountryId] = @CountryId"
            );

            CreateStoredProcedure(
               "dbo.USP_Company_Delete",
               p => new
                   {
                   CountryId = p.Int(),
                   },
               body:
                   @"DELETE [dbo].[M_Company]  
                      WHERE ([CountryId] = @CountryId)"
           );

            }

        public override void Down()
            {

            DropStoredProcedure("dbo.USP_Company_Delete");
            //DropStoredProcedure("dbo.USP_Company_Update");
            DropStoredProcedure("dbo.USP_Company_Insert");
            }
        }
    }
