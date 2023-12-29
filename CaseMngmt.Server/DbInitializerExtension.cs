using CaseMngmt.Models.Account;
using CaseMngmt.Models.ApplicationRoles;
using CaseMngmt.Models.ApplicationUsers;
using CaseMngmt.Models.Database;
using CaseMngmt.Models.FileTypes;
using CaseMngmt.Models.Keywords;
using CaseMngmt.Models.Templates;
using CaseMngmt.Repository.Companies;
using CaseMngmt.Repository.CompanyTemplates;
using CaseMngmt.Repository.FileTypes;
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
                var fileTypeManager = scope.ServiceProvider.GetRequiredService<IFileTypeRepository>();

                #region Company

                var defaultCompanyGuid = Guid.Parse("A4B11694-A5C9-48D7-BF6D-F12C019482DE");
                var defaultCompany2Guid = Guid.Parse("A4B11694-A5C9-48D7-BF6D-F12C019482DD");
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

                #endregion

                #region Type

                var defaultNumbericTypeGuid = Guid.NewGuid();
                var defaultNumbericType2Guid = Guid.NewGuid();
                var defaultDateTypeGuid = Guid.NewGuid();
                var defaultDatetimeTypeGuid = Guid.NewGuid();
                var defaultCurrencyTypeGuid = Guid.NewGuid();
                var defaultAlphanumericTypeGuid = Guid.NewGuid();
                var listDefaultGuid = Guid.NewGuid();
                var listRequestTypeGuid = Guid.NewGuid();
                var listPaymentStatusGuid = Guid.NewGuid();
                var listSubmissionStatusGuid = Guid.NewGuid();

                var listType = new List<Models.Types.Type>() {
                    new Models.Types.Type
                    {
                        Id = defaultNumbericTypeGuid,
                        Name = "Numberic (Up to 6 digits)",
                        Value = "float",
                        IsDefaultType = true
                    },
                    new Models.Types.Type
                    {
                        Id = defaultNumbericType2Guid,
                        Name = "Numberic (Up to 15 digits)",
                        Value = "double",
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
                    }
                };
                typeManager.AddMultiAsync(listType).ConfigureAwait(false);
                Thread.Sleep(2000);
                #endregion

                #region FileType

                var listFileType = new List<FileType>() {
                    new FileType
                    {
                        Name = "Delivery Receipt",
                    },
                    new FileType
                    {
                        Name = "Invoice",
                    },
                    new FileType
                    {
                        Name = "Purchase Order",
                    },
                    new FileType
                    {
                        Name = "Other",
                    },
                    new FileType
                    {
                        Name = "Shipment Schedule",
                    },
                    new FileType
                    {
                        Name = "Request Form",
                    },
                    new FileType
                    {
                        Name = "Guide Map",
                    },
                    new FileType
                    {
                        Name = "Photos",
                    },
                    new FileType
                    {
                        Name = "Measurement File",
                    },
                    new FileType
                    {
                        Name = "Design File",
                    },
                    new FileType
                    {
                        Name = "Site Map",
                    },
                    new FileType
                    {
                        Name = "Report Email",
                    }
                };
                fileTypeManager.AddMultiAsync(listFileType).ConfigureAwait(false);
                Thread.Sleep(2000);
                #endregion

                #region Template

                var defaultTemplateId = Guid.NewGuid();
                templateManager.AddAsync(new Template { Id = defaultTemplateId, Name = "Default Template" });

                #endregion

                #region CompanyTemplate

                companyTemplateManager.AddAsync(new Models.CompanyTemplates.CompanyTemplate
                {
                    CompanyId = defaultCompany2Guid,
                    TemplateId = defaultTemplateId
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
                        Searchable =true,
                        DocumentSearchable = true,
                        Order = 1,
                    },
                    new Keyword
                    {
                        Name = "Address",
                        TypeId = defaultAlphanumericTypeGuid,
                        TemplateId = defaultTemplateId,
                        MaxLength = 50,
                        IsRequired =true,
                        Searchable =true,
                        Order = 2,
                    },
                    new Keyword
                    {
                        Name = "Phone Number",
                        TypeId = defaultAlphanumericTypeGuid,
                        TemplateId = defaultTemplateId,
                        MaxLength = 12,
                        IsRequired =true,
                        Searchable =true,
                        Order = 3,
                    },
                    new Keyword
                    {
                        Name = "Customer Contact Person",
                        TypeId = defaultAlphanumericTypeGuid,
                        TemplateId = defaultTemplateId,
                        MaxLength = 100,
                        IsRequired =true,
                        Searchable =true,
                        Order = 4,
                    },
                    new Keyword
                    {
                        Name = "Reception Date",
                        TypeId = defaultDatetimeTypeGuid,
                        TemplateId = defaultTemplateId,
                        MaxLength = 100,
                        IsRequired =true,
                        Searchable =true,
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
                        Searchable =true,
                        Order = 6,
                    },
                    new Keyword
                    {
                        Name = "Site Address",
                        TypeId = defaultAlphanumericTypeGuid,
                        TemplateId = defaultTemplateId,
                        MaxLength = 100,
                        IsRequired =true,
                        Searchable =true,
                        Order = 7,
                    },
                    new Keyword
                    {
                        Name = "Scheduled Date",
                        TypeId = defaultDatetimeTypeGuid,
                        TemplateId = defaultTemplateId,
                        MaxLength = 100,
                        IsRequired =true,
                        Searchable =true,
                        Order = 8,
                    },
                    new Keyword
                    {
                        Name = "Internal PIC",
                        TypeId = defaultAlphanumericTypeGuid,
                        TemplateId = defaultTemplateId,
                        MaxLength = 100,
                        IsRequired =true,
                        Searchable =true,
                        Order = 9,
                    },
                    new Keyword
                    {
                        Name = "Submission Status",
                        TypeId = listSubmissionStatusGuid,
                        TemplateId = defaultTemplateId,
                        MaxLength = 100,
                        IsRequired =true,
                        Searchable =true,
                        Order = 10,
                    },
                    new Keyword
                    {
                        Name = "Submission Date",
                        TypeId = defaultDatetimeTypeGuid,
                        TemplateId = defaultTemplateId,
                        MaxLength = 100,
                        IsRequired =true,
                        Searchable =true,
                        Order = 11,
                    },
                    new Keyword
                    {
                        Name = "Payment Status",
                        TypeId = listPaymentStatusGuid,
                        TemplateId = defaultTemplateId,
                        MaxLength = 100,
                        IsRequired =true,
                        Searchable =true,
                        Order = 12,
                    },
                    new Keyword
                    {
                        Name = "Invoice Amount",
                        TypeId = defaultCurrencyTypeGuid,
                        TemplateId = defaultTemplateId,
                        MaxLength = 100,
                        IsRequired =true,
                        Searchable =true,
                        Order = 13,
                    },
                    new Keyword
                    {
                        Name = "Invoice Date",
                        TypeId = defaultDatetimeTypeGuid,
                        TemplateId = defaultTemplateId,
                        MaxLength = 100,
                        IsRequired =true,
                        Searchable =true,
                        Order = 14,
                    },
                    new Keyword
                    {
                        Name = "Arrival Date",
                        TypeId = defaultDatetimeTypeGuid,
                        TemplateId = defaultTemplateId,
                        MaxLength = 100,
                        IsRequired =true,
                        Searchable =true,
                        Order = 15,
                    },
                    new Keyword
                    {
                        Name = "Payment Amount",
                        TypeId = defaultCurrencyTypeGuid,
                        TemplateId = defaultTemplateId,
                        MaxLength = 100,
                        IsRequired = true,
                        Searchable = true,
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
                        Searchable = true,
                        Order = 17,
                    },
                    new Keyword
                    {
                        Name = "Note",
                        TypeId = defaultAlphanumericTypeGuid,
                        TemplateId = defaultTemplateId,
                        MaxLength = 1000,
                        IsRequired =true,
                        Searchable =true,
                        Order = 18,
                    }
                };
                keywordManager.AddMultiAsync(listKeywordDefault).ConfigureAwait(false);
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

                    var user2 = new ApplicationUser
                    {
                        Email = "tanbc0901@gmail.com",
                        SecurityStamp = Guid.NewGuid().ToString(),
                        UserName = "Admin",
                        CompanyId = defaultCompany2Guid
                    };

                    userManager.CreateAsync(user2, "Admin@123");

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
