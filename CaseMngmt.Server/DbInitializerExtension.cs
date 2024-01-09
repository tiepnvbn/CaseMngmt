using CaseMngmt.Models.Account;
using CaseMngmt.Models.ApplicationRoles;
using CaseMngmt.Models.ApplicationUsers;
using CaseMngmt.Models.Database;
using CaseMngmt.Models.Keywords;
using CaseMngmt.Models.Templates;
using CaseMngmt.Repository.Companies;
using CaseMngmt.Repository.CompanyTemplates;
using CaseMngmt.Repository.Keywords;
using CaseMngmt.Repository.Templates;
using CaseMngmt.Repository.Types;
using Microsoft.AspNetCore.Identity;

namespace CaseMngmt.Server
{
    internal static class DbInitializerExtension
    {
        public static IApplicationBuilder UseItToSeedSqlServer(this IApplicationBuilder app)
        {
            ArgumentNullException.ThrowIfNull(app, nameof(app));

            using var scope = app.ApplicationServices.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<ApplicationDbContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
                var companyManager = scope.ServiceProvider.GetRequiredService<ICompanyRepository>();
                var typeManager = scope.ServiceProvider.GetRequiredService<ITypeRepository>();
                var templateManager = scope.ServiceProvider.GetRequiredService<ITemplateRepository>();
                var keywordManager = scope.ServiceProvider.GetRequiredService<IKeywordRepository>();
                var companyTemplateManager = scope.ServiceProvider.GetRequiredService<ICompanyTemplateRepository>();

                #region Company

                var defaultCompanyGuid = Guid.Parse("A4B11694-A5C9-48D7-BF6D-F12C019482DE");
                var defaultCompany2Guid = Guid.Parse("A4B11694-A5C9-48D7-BF6D-F12C019482DD");
                var defaultCompany3Guid = Guid.Parse("A4B11694-A5C9-48D7-BF6D-F12C019482DF");
                var checkData = companyManager.GetByIdAsync(defaultCompanyGuid);
                if (checkData.Result != null)
                {
                    return app;
                }
                companyManager.AddAsync(new Models.Companies.Company
                {
                    Id = defaultCompanyGuid,
                    Name = "CASE Company DEFAULT",
                    BuildingName = "CASE Company DEFAULT",
                    City = "Ha Noi",
                    StateProvince = "Cau Giay",
                    Street = "Pham Van Bach",
                    RoomNumber = "10",
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    CreatedBy = Guid.Empty,
                    UpdatedBy = Guid.Empty,
                    PhoneNumber = "0904752252",
                    PostCode1 = "1",
                    PostCode2 = "2",
                    Deleted = false,
                });
                companyManager.AddAsync(new Models.Companies.Company
                {
                    Id = defaultCompany2Guid,
                    Name = "CASE Company",
                    BuildingName = "CASE Company",
                    City = "Ha Noi",
                    StateProvince = "Cau Giay",
                    Street = "Pham Van Bach",
                    RoomNumber = "10",
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    CreatedBy = Guid.Empty,
                    UpdatedBy = Guid.Empty,
                    PhoneNumber = "09099999999",
                    PostCode1 = "1",
                    PostCode2 = "2",
                    Deleted = false,
                });
                companyManager.AddAsync(new Models.Companies.Company
                {
                    Id = defaultCompany3Guid,
                    Name = "BOAT Company",
                    BuildingName = "BOAT Company",
                    City = "Japan",
                    StateProvince = "Tokyo",
                    Street = "",
                    RoomNumber = "",
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    CreatedBy = Guid.Empty,
                    UpdatedBy = Guid.Empty,
                    PhoneNumber = "09099999999",
                    PostCode1 = "1",
                    PostCode2 = "2",
                    Deleted = false,
                });
                #endregion

                #region Type

                var defaultNumbericTypeGuid = Guid.NewGuid();
                var defaultNumbericType2Guid = Guid.NewGuid();
                var defaultDateTypeGuid = Guid.NewGuid();
                var defaultDatetimeTypeGuid = Guid.NewGuid();
                var defaultCurrencyTypeGuid = Guid.NewGuid();
                var defaultAlphanumericTypeGuid = Guid.NewGuid();
                var listTextAreaGuid = Guid.NewGuid();
                var listDefaultGuid = Guid.NewGuid();
                var listRequestTypeGuid = Guid.NewGuid();
                var listPaymentStatusGuid = Guid.NewGuid();
                var listSubmissionStatusGuid = Guid.NewGuid();

                var listBoat1Guid = Guid.NewGuid();
                var listBoat2Guid = Guid.NewGuid();
                var listBoat3Guid = Guid.NewGuid();
                var listBoat4Guid = Guid.NewGuid();
                var listBoat5Guid = Guid.NewGuid();
                var listBoat6Guid = Guid.NewGuid();

                var listType = new List<Models.Types.Type>() {
                    new Models.Types.Type
                    {
                        Name = "Delivery Receipt",
                        Value = "string",
                        IsFileType = true,
                    },
                    new Models.Types.Type
                    {
                        Name = "Invoice",
                        Value = "string",IsFileType = true,
                    },
                    new Models.Types.Type
                    {
                        Name = "Purchase Order",Value = "string",IsFileType = true,
                    },
                    new Models.Types.Type
                    {
                        Name = "Other",Value = "string",IsFileType = true,
                    },
                    new Models.Types.Type
                    {
                        Name = "Shipment Schedule",Value = "string",IsFileType = true,
                    },
                    new Models.Types.Type
                    {
                        Name = "Request Form",Value = "string",IsFileType = true,
                    },
                    new Models.Types.Type
                    {
                        Name = "Guide Map",Value = "string",IsFileType = true,
                    },
                    new Models.Types.Type
                    {
                        Name = "Photos",Value = "string",IsFileType = true,
                    },
                    new Models.Types.Type
                    {
                        Name = "Measurement File",Value = "string",IsFileType = true,
                    },
                    new Models.Types.Type
                    {
                        Name = "Design File",Value = "string",IsFileType = true,
                    },
                    new Models.Types.Type
                    {
                        Name = "Site Map",Value = "string",IsFileType = true,
                    },
                    new Models.Types.Type
                    {
                        Name = "Report Email",Value = "string",IsFileType = true,
                    },
                    new Models.Types.Type
                    {
                        Id = defaultNumbericTypeGuid,
                        Name = "Numberic (Up to 9 digits)",
                        Value = "int",
                        IsDefaultType = true
                    },
                    new Models.Types.Type
                    {
                        Id = defaultNumbericType2Guid,
                        Name = "Numberic (Up to 20 digits)",
                        Value = "decimal",
                        IsDefaultType = true
                    },
                    new Models.Types.Type
                    {
                        Id=defaultDateTypeGuid,
                        Name = "Date",
                        Value = "datetime",
                        IsDefaultType = true
                    },
                    new Models.Types.Type
                    {
                        Id= defaultDatetimeTypeGuid,
                        Name = "Datetime",
                        Value = "datetime",
                        IsDefaultType = true
                    },
                    new Models.Types.Type
                    {
                        Id= defaultCurrencyTypeGuid,
                        Name = "Currency",
                        Value = "decimal",
                        IsDefaultType = true
                    },
                    new Models.Types.Type
                    {
                        Id = defaultAlphanumericTypeGuid,
                        Name = "Alphanumeric",
                        Value = "string",
                        IsDefaultType = true
                    },
                    new Models.Types.Type
                    {
                        Id = listTextAreaGuid,
                        Name = "TextArea",
                        Value = "textarea",
                        IsDefaultType = true
                    },
                    new Models.Types.Type
                    {
                        Id = listDefaultGuid,
                        Name = "List (Alphanumeric)",
                        Value = "list",
                        IsDefaultType = true
                    },
                    new Models.Types.Type
                    {
                        Id = listRequestTypeGuid,
                        Name = "List (Alphanumeric)",
                        Value = "list",
                        Metadata = "Land Parcel Survey,Topographic Survey,Geological Survey,Seismic Survey,Groundwater Survey,Architectural Design,New Construction,Remodeling/Renovation Construction,Demolition Construction",
                    },
                    new Models.Types.Type
                    {
                        Id = listSubmissionStatusGuid,
                        Name = "List (Alphanumeric)",
                        Value = "list",
                        Metadata = "Not yet,Done",
                    },
                    new Models.Types.Type
                    {
                        Id = listPaymentStatusGuid,
                        Name = "List (Alphanumeric)",
                        Value = "list",
                        Metadata = "Billed,Check Payment,Complete",
                    },
                    new Models.Types.Type
                    {
                        Id = listBoat1Guid,
                        Name = "List 注文種類",
                        Value = "list",
                        Metadata = "ジェット,バスボート,マリンボート,OEM,その他",
                    },
                    new Models.Types.Type
                    {
                        Id = listBoat2Guid,
                        Name = "List 採寸性",
                        Value = "list",
                        Metadata = "不要,必要",
                    },
                    new Models.Types.Type
                    {
                        Id = listBoat3Guid,
                        Name = "List 裁断状況",
                        Value = "list",
                        Metadata = "未完了,完了",
                    },
                    new Models.Types.Type
                    {
                        Id = listBoat4Guid,
                        Name = "List 納期状況",
                        Value = "list",
                        Metadata = "未完了,完了",
                    },
                    new Models.Types.Type
                    {
                        Id = listBoat5Guid,
                        Name = "List 社内担当名",
                        Value = "list",
                        Metadata = "田中太郎,山田花子,佐藤雅人,鈴木美咲,小林太一",
                    },
                    new Models.Types.Type
                    {
                        Id = listBoat6Guid,
                        Name = "List 入金状況",
                        Value = "list",
                        Metadata = "請求済,入金要確認,入金完了",
                    }
                };
                typeManager.AddMultiAsync(listType).ConfigureAwait(false);
                Thread.Sleep(5000);

                #endregion

                #region Template

                var defaultTemplateId = Guid.NewGuid();
                var boatTemplateId = Guid.NewGuid();
                templateManager.AddAsync(new Template { Id = defaultTemplateId, Name = "Default Template" });
                templateManager.AddAsync(new Template { Id = boatTemplateId, Name = "BOAT Template" });

                #endregion

                #region CompanyTemplate

                companyTemplateManager.AddAsync(new Models.CompanyTemplates.CompanyTemplate
                {
                    CompanyId = defaultCompany2Guid,
                    TemplateId = defaultTemplateId
                });
                companyTemplateManager.AddAsync(new Models.CompanyTemplates.CompanyTemplate
                {
                    CompanyId = defaultCompany3Guid,
                    TemplateId = boatTemplateId
                });

                #endregion

                #region Keyword

                var listKeywordDefault = new List<Keyword> {
                    new Keyword
                    {
                        Name = "Customer Name",
                        TypeId = defaultAlphanumericTypeGuid,
                        TemplateId = defaultTemplateId,
                        MaxLength = 20,
                        IsRequired =true,
                        CaseSearchable =true,
                        DocumentSearchable = true,
                        Order = 1,
                        IsShowOnCaseList = true
                    },
                    new Keyword
                    {
                        Name = "Address",
                        TypeId = defaultAlphanumericTypeGuid,
                        TemplateId = defaultTemplateId,
                        MaxLength = 50,
                        IsRequired =true,
                        CaseSearchable =true,
                        Order = 2,
                    },
                    new Keyword
                    {
                        Name = "Phone Number",
                        TypeId = defaultAlphanumericTypeGuid,
                        TemplateId = defaultTemplateId,
                        MaxLength = 12,
                        IsRequired =true,
                        CaseSearchable =true,
                        Order = 3,
                    },
                    new Keyword
                    {
                        Name = "Customer Contact Person",
                        TypeId = defaultAlphanumericTypeGuid,
                        TemplateId = defaultTemplateId,
                        MaxLength = 100,
                        IsRequired =true,
                        CaseSearchable =true,
                        Order = 4,
                    },
                    new Keyword
                    {
                        Name = "Reception Date",
                        TypeId = defaultDatetimeTypeGuid,
                        TemplateId = defaultTemplateId,
                        MaxLength = 100,
                        IsRequired =true,
                        CaseSearchable =true,
                        DocumentSearchable = true,
                        Order = 5,
                    },
                    new Keyword
                    {
                        Name = "Request Type",
                        TypeId = listRequestTypeGuid,
                        TemplateId = defaultTemplateId,
                        MaxLength = 100,
                        IsRequired =true,
                        CaseSearchable =true,
                        Order = 6,
                        IsShowOnCaseList = true
                    },
                    new Keyword
                    {
                        Name = "Site Address",
                        TypeId = defaultAlphanumericTypeGuid,
                        TemplateId = defaultTemplateId,
                        MaxLength = 100,
                        IsRequired =true,
                        CaseSearchable =true,
                        Order = 7,
                    },
                    new Keyword
                    {
                        Name = "Scheduled Date",
                        TypeId = defaultDatetimeTypeGuid,
                        TemplateId = defaultTemplateId,
                        MaxLength = 100,
                        IsRequired =true,
                        CaseSearchable =true,
                        Order = 8,
                    },
                    new Keyword
                    {
                        Name = "Internal PIC",
                        TypeId = defaultAlphanumericTypeGuid,
                        TemplateId = defaultTemplateId,
                        MaxLength = 100,
                        IsRequired =true,
                        CaseSearchable =true,
                        Order = 9,
                        IsShowOnCaseList = true
                    },
                    new Keyword
                    {
                        Name = "Submission Status",
                        TypeId = listSubmissionStatusGuid,
                        TemplateId = defaultTemplateId,
                        MaxLength = 100,
                        IsRequired =true,
                        CaseSearchable =true,
                        Order = 10,
                    },
                    new Keyword
                    {
                        Name = "Submission Date",
                        TypeId = defaultDatetimeTypeGuid,
                        TemplateId = defaultTemplateId,
                        MaxLength = 100,
                        IsRequired =true,
                        CaseSearchable =true,
                        Order = 11,
                    },
                    new Keyword
                    {
                        Name = "Payment Status",
                        TypeId = listPaymentStatusGuid,
                        TemplateId = defaultTemplateId,
                        MaxLength = 100,
                        IsRequired =true,
                        CaseSearchable =true,
                        Order = 12,
                    },
                    new Keyword
                    {
                        Name = "Invoice Amount",
                        TypeId = defaultCurrencyTypeGuid,
                        TemplateId = defaultTemplateId,
                        MaxLength = 100,
                        IsRequired =true,
                        CaseSearchable =true,
                        Order = 13,
                    },
                    new Keyword
                    {
                        Name = "Invoice Date",
                        TypeId = defaultDatetimeTypeGuid,
                        TemplateId = defaultTemplateId,
                        MaxLength = 100,
                        IsRequired =true,
                        CaseSearchable =true,
                        Order = 14,
                    },
                    new Keyword
                    {
                        Name = "Arrival Date",
                        TypeId = defaultDatetimeTypeGuid,
                        TemplateId = defaultTemplateId,
                        MaxLength = 100,
                        IsRequired =true,
                        CaseSearchable =true,
                        Order = 15,
                    },
                    new Keyword
                    {
                        Name = "Payment Amount",
                        TypeId = defaultCurrencyTypeGuid,
                        TemplateId = defaultTemplateId,
                        MaxLength = 100,
                        IsRequired = true,
                        CaseSearchable = true,
                        DocumentSearchable = true,
                        Order = 16,
                    },
                    new Keyword
                    {
                        Name = "Payment Date",
                        TypeId = defaultDatetimeTypeGuid,
                        TemplateId = defaultTemplateId,
                        MaxLength = 100,
                        IsRequired = true,
                        CaseSearchable = true,
                        Order = 17,
                    },
                    new Keyword
                    {
                        Name = "Note",
                        TypeId = listTextAreaGuid,
                        TemplateId = defaultTemplateId,
                        MaxLength = 1000,
                        IsRequired =true,
                        CaseSearchable =true,
                        Order = 18,
                    }
                };
                keywordManager.AddMultiAsync(listKeywordDefault).ConfigureAwait(false);
                Thread.Sleep(2000);

                var listKeywordBoat = new List<Keyword> {
                    new Keyword
                    {
                        Name = "取引先名",
                        TypeId = defaultAlphanumericTypeGuid,
                        TemplateId = boatTemplateId,
                        MaxLength = 250,
                        IsRequired = true,
                        CaseSearchable = true,
                        DocumentSearchable = true,
                        Order = 1,
                        IsShowOnCaseList = true
                    },
                    new Keyword
                    {
                        Name = "住所",
                        TypeId = defaultAlphanumericTypeGuid,
                        TemplateId = boatTemplateId,
                        MaxLength = 250,
                        IsRequired = false,
                        CaseSearchable = false,
                        Order = 2,
                    },
                    new Keyword
                    {
                        Name = "電話番号",
                        TypeId = defaultNumbericType2Guid,
                        TemplateId = boatTemplateId,
                        IsRequired = true,
                        CaseSearchable = true,
                        Order = 3,
                    },
                    new Keyword
                    {
                        Name = "顧客担当者名",
                        TypeId = defaultAlphanumericTypeGuid,
                        TemplateId = boatTemplateId,
                        MaxLength = 20,
                        Order = 4,
                    },
                    new Keyword
                    {
                        Name = "注文日",
                        TypeId = defaultDatetimeTypeGuid,
                        TemplateId = boatTemplateId,
                        IsRequired = true,
                        CaseSearchable = true,
                        DocumentSearchable = true,
                        Order = 5,
                    },
                    new Keyword
                    {
                        Name = "注文種類",
                        TypeId = listBoat1Guid,
                        TemplateId = boatTemplateId,
                        MaxLength = 50,
                        IsRequired = true,
                        CaseSearchable = true,
                        Order = 6,
                        IsShowOnCaseList = true
                    },
                    new Keyword
                    {
                        Name = "商品名/内容",
                        TypeId = defaultAlphanumericTypeGuid,
                        TemplateId = boatTemplateId,
                        MaxLength = 250,
                        IsRequired = true,
                        Order = 7,
                    },
                    new Keyword
                    {
                        Name = "生地タイプ",
                        TypeId = defaultAlphanumericTypeGuid,
                        TemplateId = boatTemplateId,
                        MaxLength = 20,
                        IsRequired = true,
                        Order = 8,
                    },
                    new Keyword
                    {
                        Name = "生地色",
                        TypeId = defaultAlphanumericTypeGuid,
                        TemplateId = boatTemplateId,
                        MaxLength = 20,
                        IsRequired = true,
                        Order = 9,
                    },
                    new Keyword
                    {
                        Name = "数量",
                        TypeId = defaultNumbericTypeGuid,
                        TemplateId = boatTemplateId,
                        IsRequired = true,
                        Order = 10,
                    },
                    new Keyword
                    {
                        Name = "単価",
                        TypeId = defaultCurrencyTypeGuid,
                        TemplateId = boatTemplateId,
                        Order = 11,
                    },
                    new Keyword
                    {
                        Name = "金額",
                        TypeId = defaultCurrencyTypeGuid,
                        TemplateId = boatTemplateId,
                        DocumentSearchable = true,
                        Order = 12,
                    },
                    new Keyword
                    {
                        Name = "納期要望日",
                        TypeId = defaultDatetimeTypeGuid,
                        TemplateId = boatTemplateId,
                        Order = 13,
                    },
                    new Keyword
                    {
                        Name = "採寸性",
                        TypeId = listBoat2Guid,
                        TemplateId = boatTemplateId,
                        MaxLength = 50,
                        Order = 14,
                    },
                    new Keyword
                    {
                        Name = "裁断状況",
                        TypeId = listBoat3Guid,
                        TemplateId = boatTemplateId,
                        MaxLength = 10,
                        Order = 15,
                    },
                    new Keyword
                    {
                        Name = "納期状況",
                        TypeId = listBoat4Guid,
                        TemplateId = boatTemplateId,
                        MaxLength = 10,
                        Order = 16,
                    },
                    new Keyword
                    {
                        Name = "社内担当名",
                        TypeId = listBoat5Guid,
                        TemplateId = boatTemplateId,
                        MaxLength = 20,
                        CaseSearchable = true,
                        Order = 17,
                    },
                    new Keyword
                    {
                        Name = "入金状況",
                        TypeId = listBoat6Guid,
                        TemplateId = boatTemplateId,
                        MaxLength = 10,
                        CaseSearchable = true,
                        Order = 18,
                    },
                    new Keyword
                    {
                        Name = "請求日",
                        TypeId = defaultDatetimeTypeGuid,
                        TemplateId = boatTemplateId,
                        CaseSearchable = true,
                        Order = 19,
                    },
                    new Keyword
                    {
                        Name = "入金日",
                        TypeId = defaultDatetimeTypeGuid,
                        TemplateId = boatTemplateId,
                        CaseSearchable = true,
                        Order = 20,
                    },
                    new Keyword
                    {
                        Name = "備考",
                        TypeId = listTextAreaGuid,
                        TemplateId = boatTemplateId,
                        Order = 21,
                    }
                };
                keywordManager.AddMultiAsync(listKeywordBoat).ConfigureAwait(false);
                Thread.Sleep(2000);

                #endregion

                #region Users & Roles

                var userExists = userManager.FindByNameAsync("SuperAdmin");
                if (userExists.Result == null)
                {
                    var user = new ApplicationUser
                    {
                        Email = "hoangthanhduong@gmail.com",
                        SecurityStamp = Guid.NewGuid().ToString(),
                        UserName = "SuperAdmin",
                        CompanyId = defaultCompanyGuid
                    };
                    userManager.CreateAsync(user, "Admin@123");
                    Thread.Sleep(2000);

                    var user2 = new ApplicationUser
                    {
                        Email = "tanbc0901@gmail.com",
                        SecurityStamp = Guid.NewGuid().ToString(),
                        UserName = "Admin",
                        CompanyId = defaultCompany2Guid
                    };
                    userManager.CreateAsync(user2, "Admin@123");
                    Thread.Sleep(2000);

                    var boatUser = new ApplicationUser
                    {
                        Email = "demo@gmail.com",
                        SecurityStamp = Guid.NewGuid().ToString(),
                        UserName = "BoatAdmin",
                        CompanyId = defaultCompany3Guid
                    };
                    userManager.CreateAsync(boatUser, "Admin@123");
                    Thread.Sleep(2000);

                    var roleSuperAdminCheck = roleManager.RoleExistsAsync("SuperAdmin");
                    if (!roleSuperAdminCheck.Result)
                    {
                        roleManager.CreateAsync(new ApplicationRole(UserRoles.SuperAdmin));
                    }
                    var roleAdminCheck = roleManager.RoleExistsAsync("Admin");
                    if (!roleAdminCheck.Result)
                    {
                        roleManager.CreateAsync(new ApplicationRole(UserRoles.Admin));
                    }

                    Thread.Sleep(2000);
                    userManager.AddToRoleAsync(user, "SuperAdmin");
                    Thread.Sleep(2000);
                    userManager.AddToRoleAsync(user2, "Admin");
                    Thread.Sleep(2000);
                    userManager.AddToRoleAsync(boatUser, "Admin");
                    Thread.Sleep(2000);

                    #endregion

                }
            }
            catch (Exception ex)
            {

            }
            return app;
        }
    }
}
