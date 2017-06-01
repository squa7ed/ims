using IMS.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace IMS
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Log.Register();
            InitializeData();
        }

        public void InitializeAllData()
        {
            InitializeData();
        }


        public void InitializeData()
        {
            if (Repository<Age>.Get().Count() == 0)
            {
                var list = new List<Age>();
                var root = new Age() { Name = "中国历史学年代" };
                var ndbx = new Age() { Name = "年代不详" };
                list.Add(root);
                list.Add(ndbx);
                list.Add(new Age() { Name = "夏(约前2070~前1600)", Parent = root });
                list.Add(new Age() { Name = "商(前1600~前1046)", Parent = root });
                list.Add(new Age() { Name = "秦(前221~前206)", Parent = root });
                list.Add(new Age() { Name = "西晋(265~317)", Parent = root });
                list.Add(new Age() { Name = "隋(581~618)", Parent = root });
                list.Add(new Age() { Name = "唐(618~907)", Parent = root });
                list.Add(new Age() { Name = "辽(907~1125)", Parent = root });
                list.Add(new Age() { Name = "西夏(1038~1227)", Parent = root });
                list.Add(new Age() { Name = "金(1115~1234)", Parent = root });
                list.Add(new Age() { Name = "元(1206~1368)", Parent = root });
                list.Add(new Age() { Name = "明(1368~1644)", Parent = root });
                list.Add(new Age() { Name = "清(1616~1911)", Parent = root });
                list.Add(new Age() { Name = "中华民国(1912~1949)", Parent = root });
                list.Add(new Age() { Name = "中华人民共和国(1949年10月1日成立)", Parent = root });

                var zhou = new Age() { Name = "周", Parent = root };
                list.Add(zhou);
                list.Add(new Age() { Name = "西周(前1046~前771)", Parent = zhou });
                list.Add(new Age() { Name = "东周(前770~前256)", Parent = zhou });
                list.Add(new Age() { Name = "春秋时代(前770~前476)", Parent = zhou });
                list.Add(new Age() { Name = "战国时代(前475~前221)", Parent = zhou });

                var han = new Age() { Name = "汉", Parent = root };
                list.Add(han);
                list.Add(new Age() { Name = "西汉(前206~公元25)", Parent = han });
                list.Add(new Age() { Name = "东汉(25~220)", Parent = han });

                var sanguo = new Age() { Name = "三国", Parent = root };
                list.Add(sanguo);
                list.Add(new Age() { Name = "三国·魏(220~265)", Parent = sanguo });
                list.Add(new Age() { Name = "三国·蜀(221~263)", Parent = sanguo });
                list.Add(new Age() { Name = "三国·吴(222~280)", Parent = sanguo });

                var djslg = new Age() { Name = "东晋十六国", Parent = root };
                list.Add(djslg);
                list.Add(new Age() { Name = "东晋(317~420)", Parent = djslg });
                list.Add(new Age() { Name = "十六国(304~439)", Parent = djslg });

                var nbc = new Age() { Name = "南北朝", Parent = root };
                list.Add(nbc);
                var nc = new Age() { Name = "南朝", Parent = nbc };
                list.Add(nc);
                var bc = new Age() { Name = "北朝", Parent = nbc };
                list.Add(bc);
                list.Add(new Age() { Name = "南朝·宋(420~479)", Parent = nc });
                list.Add(new Age() { Name = "南朝·齐(479~502)", Parent = nc });
                list.Add(new Age() { Name = "南朝·梁(502~557)", Parent = nc });
                list.Add(new Age() { Name = "南朝·陈(557~589)", Parent = nc });
                list.Add(new Age() { Name = "北魏(386~534)", Parent = bc });
                list.Add(new Age() { Name = "东魏(534~550)", Parent = bc });
                list.Add(new Age() { Name = "北齐(550~577)", Parent = bc });
                list.Add(new Age() { Name = "北周(557~581)", Parent = bc });

                var wdsg = new Age() { Name = "五代十国", Parent = root };
                list.Add(wdsg);
                list.Add(new Age() { Name = "五代·后梁(907~923)", Parent = wdsg });
                list.Add(new Age() { Name = "五代·后唐(923~936)", Parent = wdsg });
                list.Add(new Age() { Name = "五代·后晋(936~947)", Parent = wdsg });
                list.Add(new Age() { Name = "五代·后汉(947~950)", Parent = wdsg });
                list.Add(new Age() { Name = "五代•后周(951~960)", Parent = wdsg });
                list.Add(new Age() { Name = "十国(902~979)", Parent = wdsg });

                var song = new Age() { Name = "宋", Parent = root };
                list.Add(song);
                list.Add(new Age() { Name = "北宋(960~1127)", Parent = wdsg });
                list.Add(new Age() { Name = "南宋(1127~1279)", Parent = wdsg });
                list.ForEach(x => Repository<Age>.Add(x));
                foreach (var age in Repository<Age>.Get())
                {
                    if (age.Parent != null)
                    {
                        if (age.Parent.Children == null)
                        {
                            age.Parent.Children = new HashSet<Age>();
                        }
                        age.Parent.Children.Add(age);
                    }
                }
            }

            if (Repository<Author>.Get().Count() == 0)
            {
                Repository<Author>.Add(new Author() { Name = "朱屺瞻" });
            }
            if (Repository<Category>.Get().Count() == 0)
            {
                new List<Category>()
                {
                    new Category(){ Name = "玉石器、宝石" },
                    new Category(){ Name = "陶器" },
                    new Category(){ Name = "瓷器" },
                    new Category(){ Name = "铜器" },
                    new Category(){ Name = "金银器" },
                    new Category(){ Name = "铁器、其他金属器" },
                    new Category(){ Name = "漆器" },
                    new Category(){ Name = "雕塑、造像" },
                    new Category(){ Name = "石器、石刻、砖瓦" },
                    new Category(){ Name = "书法、绘画" },
                    new Category(){ Name = "文具" },
                    new Category(){ Name = "甲骨" },
                    new Category(){ Name = "玺印符牌" },
                    new Category(){ Name = "钱币" },
                    new Category(){ Name = "牙骨角器" },
                    new Category(){ Name = "竹木雕" },
                    new Category(){ Name = "家具" },
                    new Category(){ Name = "珐琅器" },
                    new Category(){ Name = "织绣" },
                    new Category(){ Name = "古籍图书" },
                    new Category(){ Name = "碑帖拓本" },
                    new Category(){ Name = "武器" },
                    new Category(){ Name = "邮品" },
                    new Category(){ Name = "文件、宣传品" },
                    new Category(){ Name = "档案文书" },
                    new Category(){ Name = "名人遗物" },
                    new Category(){ Name = "玻璃器" },
                    new Category(){ Name = "乐器、法器" },
                    new Category(){ Name = "皮革" },
                    new Category(){ Name = "音像制品" },
                    new Category(){ Name = "票据" },
                    new Category(){ Name = "交通、运输工具" },
                    new Category(){ Name = "度量衡器" },
                    new Category(){ Name = "标本、化石" },
                    new Category(){ Name = "其他" }
                }.ForEach(p => Repository<Category>.Add(p));
            }
            if (Repository<CollectedTimeRange>.Get().Count() == 0)
            {
                new List<CollectedTimeRange>()
                {
                    new CollectedTimeRange(){ Name = "1949.10.1前" },
                    new CollectedTimeRange(){ Name = "1949.10.1-1965" },
                    new CollectedTimeRange(){ Name = "1966-1976" },
                    new CollectedTimeRange(){ Name = "1977-2000" },
                    new CollectedTimeRange(){ Name = "2001至今" }
                }.ForEach(p => Repository<CollectedTimeRange>.Add(p));
            }
            if (Repository<DisabilityLevel>.Get().Count() == 0)
            {
                new List<DisabilityLevel>()
                {
                    new DisabilityLevel(){ Name = "完整" },
                    new DisabilityLevel(){ Name = "基本完整" },
                    new DisabilityLevel(){ Name = "残缺" },
                    new DisabilityLevel(){ Name = "严重残缺(含缺失部件)" }
                }.ForEach(p => Repository<DisabilityLevel>.Add(p));
            }
            if (Repository<Grain>.Get().Count() == 0)
            {
                var list = new List<Grain>();
                var dyzd = new Grain() { Name = "单一质地" };
                list.Add(dyzd);
                var yjz = new Grain() { Name = "有机质", Parent = dyzd };
                list.Add(yjz);
                list.Add(new Grain() { Name = "木", Parent = yjz });
                list.Add(new Grain() { Name = "竹", Parent = yjz });
                list.Add(new Grain() { Name = "纸", Parent = yjz });
                list.Add(new Grain() { Name = "毛", Parent = yjz });
                list.Add(new Grain() { Name = "丝", Parent = yjz });
                list.Add(new Grain() { Name = "皮革", Parent = yjz });
                list.Add(new Grain() { Name = "骨角牙", Parent = yjz });
                list.Add(new Grain() { Name = "棉麻纤维", Parent = yjz });
                list.Add(new Grain() { Name = "其他植物质", Parent = yjz });
                list.Add(new Grain() { Name = "其他动物质", Parent = yjz });
                list.Add(new Grain() { Name = "其他有机质", Parent = yjz });

                var wjz = new Grain() { Name = "无机质", Parent = dyzd };
                list.Add(wjz);
                list.Add(new Grain() { Name = "石", Parent = wjz });
                list.Add(new Grain() { Name = "瓷", Parent = wjz });
                list.Add(new Grain() { Name = "砖瓦", Parent = wjz });
                list.Add(new Grain() { Name = "泥", Parent = wjz });
                list.Add(new Grain() { Name = "陶", Parent = wjz });
                list.Add(new Grain() { Name = "玻璃", Parent = wjz });
                list.Add(new Grain() { Name = "铁", Parent = wjz });
                list.Add(new Grain() { Name = "铜", Parent = wjz });
                list.Add(new Grain() { Name = "宝玉石", Parent = wjz });
                list.Add(new Grain() { Name = "金", Parent = wjz });
                list.Add(new Grain() { Name = "银", Parent = wjz });
                list.Add(new Grain() { Name = "其他金属", Parent = wjz });
                list.Add(new Grain() { Name = "其他无机质", Parent = wjz });
                var fh = new Grain() { Name = "复合或组合质地" };
                list.Add(fh);
                list.Add(new Grain() { Name = "有机复合或组合", Parent = fh });
                list.Add(new Grain() { Name = "无机复合或组合", Parent = fh });
                list.Add(new Grain() { Name = "有机无机复合或组合", Parent = fh });
                list.ForEach(x => Repository<Grain>.Add(x));
                foreach (var grain in Repository<Grain>.Get())
                {
                    if (grain.Parent != null)
                    {
                        if (grain.Parent.Children == null)
                        {
                            grain.Parent.Children = new HashSet<Grain>();
                        }
                        grain.Parent.Children.Add(grain);
                    }
                }
            }
            if (Repository<Level>.Get().Count() == 0)
            {
                new List<Level>()
                {
                    new Level(){ Name = "一级" },
                    new Level(){ Name = "二级" },
                    new Level(){ Name = "三级" },
                    new Level(){ Name = "一般" },
                    new Level(){ Name = "未定级" }
                }.ForEach(p => Repository<Level>.Add(p));
            }
            if (Repository<Location>.Get().Count() == 0)
            {
                new List<Location>()
                {
                    new Location(){ Name = "研究所" },
                    new Location(){ Name = "所内书籍储藏室" },
                    new Location(){ Name = "所内文物储藏室" },
                    new Location(){ Name = "琉璃河" },
                    new Location(){ Name = "琉璃河仓库" }
                }.ForEach(x => Repository<Location>.Add(x));
            }
            if (Repository<RelicIdType>.Get().Count() == 0)
            {
                new List<RelicIdType>()
                {
                    new RelicIdType(){ Name = "藏品总登记号" },
                    new RelicIdType(){ Name = "辅助账号" },
                    new RelicIdType(){ Name = "索书号" },
                    new RelicIdType(){ Name = "档案编号" },
                    new RelicIdType(){ Name = "固定资产登记号" },
                    new RelicIdType(){ Name = "财产登记号" },
                    new RelicIdType(){ Name = "出土(水)登记号" },
                    new RelicIdType(){ Name = "其他编号" }
                }.ForEach(p => Repository<RelicIdType>.Add(p));
            }
            if (Repository<Source>.Get().Count() == 0)
            {
                new List<Source>()
                {
                    new Source(){ Name = "征集购买" },
                    new Source(){ Name = "接受捐赠" },
                    new Source(){ Name = "依法交换" },
                    new Source(){ Name = "拨交" },
                    new Source(){ Name = "移交" },
                    new Source(){ Name = "旧藏" },
                    new Source(){ Name = "发掘" },
                    new Source(){ Name = "采集" },
                    new Source(){ Name = "拣选" },
                    new Source(){ Name = "其他" }

                }.ForEach(p => Repository<Source>.Add(p));
            }
            if (Repository<StoringCondition>.Get().Count() == 0)
            {
                new List<StoringCondition>()
                {
                    new StoringCondition(){ Name = "状态稳定，不需修复" },
                    new StoringCondition(){ Name = "部分损腐，需要修复" },
                    new StoringCondition(){ Name = "腐蚀损毁严重，急需修复" },
                    new StoringCondition(){ Name = "已修复" }
                }.ForEach(p => Repository<StoringCondition>.Add(p));
            }
            if (Repository<Unit>.Get().Count() == 0)
            {
                new List<Unit>()
                {
                    new Unit(){ Name ="cm", Type = UnitTypes.Length },
                    new Unit(){ Name ="m", Type = UnitTypes.Length },
                    new Unit(){ Name ="g", Type = UnitTypes.Weight },
                    new Unit(){ Name ="kg", Type = UnitTypes.Weight  },
                    new Unit(){ Name ="件", Type = UnitTypes.Relic},
                    new Unit(){ Name ="本", Type = UnitTypes.Relic },
                    new Unit(){ Name ="枚", Type = UnitTypes.Relic},
                }.ForEach(x => Repository<Unit>.Add(x));
            }
            if (Repository<WeightRange>.Get().Count() == 0)
            {
                new List<WeightRange>()
                {
                    new WeightRange(){ Name = "<0.01 kg" },
                    new WeightRange(){ Name = "0.01-1 kg" },
                    new WeightRange(){ Name = "1-50 kg" },
                    new WeightRange(){ Name = "50-100 kg" },
                    new WeightRange(){ Name = "100-1000 kg" },
                    new WeightRange(){ Name = ">1000 kg" }
                }.ForEach(p => Repository<WeightRange>.Add(p));
            }
            if (Repository<Storage>.Get().Count() == 0)
            {
                var list = new List<Storage>();
                var sjccs = new Storage() { Name = "所内书籍储藏室", Location = Repository<Location>.Get().Single(x => x.Name == "研究所") };
                list.Add(sjccs);
                list.Add(new Storage()
                {
                    Name = "储物架1",
                    Location = Repository<Location>.Get().FirstOrDefault(x => x.Name == "所内书籍储藏室"),
                    Parent = sjccs
                });
                list.Add(new Storage()
                {
                    Name = "储物架2",
                    Location = Repository<Location>.Get().FirstOrDefault(x => x.Name == "所内书籍储藏室"),
                    Parent = sjccs
                });
                list.Add(new Storage()
                {
                    Name = "储物架3",
                    Location = Repository<Location>.Get().FirstOrDefault(x => x.Name == "所内书籍储藏室"),
                    Parent = sjccs
                });
                list.Add(new Storage()
                {
                    Name = "储物架4",
                    Location = Repository<Location>.Get().FirstOrDefault(x => x.Name == "所内书籍储藏室"),
                    Parent = sjccs
                });
                list.Add(new Storage()
                {
                    Name = "储物架5",
                    Location = Repository<Location>.Get().FirstOrDefault(x => x.Name == "所内书籍储藏室"),
                    Parent = sjccs
                });

                var wwccs = new Storage() { Name = "所内文物储藏室", Location = Repository<Location>.Get().Single(x => x.Name == "研究所") };
                list.Add(wwccs);
                list.Add(new Storage()
                {
                    Name = "储物架1",
                    Location = Repository<Location>.Get().FirstOrDefault(x => x.Name == "所内文物储藏室"),
                    Parent = wwccs
                });
                list.Add(new Storage()
                {
                    Name = "储物架2",
                    Location = Repository<Location>.Get().FirstOrDefault(x => x.Name == "所内文物储藏室"),
                    Parent = wwccs
                });
                list.Add(new Storage()
                {
                    Name = "储物架3",
                    Location = Repository<Location>.Get().FirstOrDefault(x => x.Name == "所内文物储藏室"),
                    Parent = wwccs
                });
                list.Add(new Storage()
                {
                    Name = "储物架4",
                    Location = Repository<Location>.Get().FirstOrDefault(x => x.Name == "所内文物储藏室"),
                    Parent = wwccs
                });
                list.Add(new Storage()
                {
                    Name = "储物架5",
                    Location = Repository<Location>.Get().FirstOrDefault(x => x.Name == "所内文物储藏室"),
                    Parent = wwccs
                });

                var llhck = new Storage() { Name = "琉璃河仓库", Location = Repository<Location>.Get().Single(x => x.Name == "研究所") };
                list.Add(llhck);
                list.Add(new Storage()
                {
                    Name = "储物架1",
                    Location = Repository<Location>.Get().FirstOrDefault(x => x.Name == "琉璃河仓库"),
                    Parent = llhck
                });
                list.Add(new Storage()
                {
                    Name = "储物架2",
                    Location = Repository<Location>.Get().FirstOrDefault(x => x.Name == "琉璃河仓库"),
                    Parent = llhck
                });
                list.Add(new Storage()
                {
                    Name = "储物架3",
                    Location = Repository<Location>.Get().FirstOrDefault(x => x.Name == "琉璃河仓库"),
                    Parent = llhck
                });
                list.Add(new Storage()
                {
                    Name = "储物架4",
                    Location = Repository<Location>.Get().FirstOrDefault(x => x.Name == "琉璃河仓库"),
                    Parent = llhck
                });
                list.Add(new Storage()
                {
                    Name = "储物架5",
                    Location = Repository<Location>.Get().FirstOrDefault(x => x.Name == "琉璃河仓库"),
                    Parent = llhck
                });
                list.ForEach(x => Repository<Storage>.Add(x));
            }
            if (Repository<ReceiptType>.Get().Count() == 0)
            {
                new List<ReceiptType>()
                {
                    new ReceiptType()
                    {
                        Type = ReceiptTypes.NewReceipt,
                        Name = "新增入库"
                    },
                    new ReceiptType()
                    {
                        Type = ReceiptTypes.Return,
                        Name = "提借归还"
                    },
                    new ReceiptType()
                    {
                        Type = ReceiptTypes.BorrowFromOuterUnit,
                        Name = "借用外馆"
                    }
                }.ForEach(x => Repository<ReceiptType>.Add(x));
            }
            if (Repository<DeliveryType>.Get().Count() == 0)
            {
                new List<DeliveryType>()
                {
                    new DeliveryType()
                    {
                        Type = DeliveryTypes.Borrow,
                        Name = "藏品提借"
                    },
                    new DeliveryType()
                    {
                        Type = DeliveryTypes.ReturnToOutUnit,
                        Name = "归还外馆"
                    },
                    new DeliveryType()
                    {
                        Type = DeliveryTypes.Unregister,
                        Name = "藏品注销"
                    }
                }.ForEach(x => Repository<DeliveryType>.Add(x));
            }
            if (Repository<Department>.Get().Count() == 0)
            {
                new List<Department>()
                {
                    new Department() { Name = "综合科"},
                    new Department() { Name = "宣传科"},
                    new Department() { Name = "后勤科"},
                    new Department() { Name = "仓库管理科"},
                    new Department() { Name = "古籍研究室"}
                }.ForEach(x => Repository<Department>.Add(x));
            }
            if (Repository<User>.Get().Count() == 0)
            {
                new List<User>()
                {
                    new User()
                    {
                        Name = "admin",
                        Password = "123456",
                        Department =   Repository<Department>.Get().First(x => x.Name == "综合科")
                    },
                    new User()
                    {
                        Name = "张某",
                        Password = "123456",
                        Department =   Repository<Department>.Get().First(x => x.Name == "综合科")
                    },
                    new User()
                    {
                        Name = "李某",
                        Password = "123456",
                        Department =   Repository<Department>.Get().First(x => x.Name == "宣传科")
                    },
                   new User()
                   {
                       Name = "刘某",
                       Password = "123456",
                        Department =   Repository<Department>.Get().First(x => x.Name == "宣传科")
                   },
                    new User()
                    {
                        Name = "赵某",
                        Password = "123456",
                        Department =   Repository<Department>.Get().First(x => x.Name == "后勤科")
                    },
                    new User()
                    {
                        Name = "徐某",
                        Password = "123456",
                        Department =   Repository<Department>.Get().First(x => x.Name == "后勤科")
                    },
                    new User()
                    {
                        Name = "钟某",
                        Password = "123456",
                        Department =   Repository<Department>.Get().First(x => x.Name == "仓库管理科")
                    },
                    new User()
                    {
                        Name = "杨某",
                        Password = "123456",
                        Department =   Repository<Department>.Get().First(x => x.Name == "仓库管理科")
                    },
                    new User()
                    {
                        Name = "王某",
                        Password = "123456",
                        Department =   Repository<Department>.Get().First(x => x.Name == "古籍研究室")

                    },
                    new User()
                    {
                        Name = "董某",
                        Password = "123456",
                        Department =   Repository<Department>.Get().First(x => x.Name == "古籍研究室")
                    },
                    new User()
                    {
                        Name = "夏某",
                        Password = "123456",
                        Department =   Repository<Department>.Get().First(x => x.Name == "古籍研究室")
                    }
                }.ForEach(x => Repository<User>.Add(x));
            }

            //if (Repository<Relic>.Get().Count() == 0)
            //{
            //    for (int i = 0; i < 10; i++)
            //    {
            //        Repository<Relic>.Add(new Relic()
            //        {
            //            RootAge = Repository<Age>.Get().First(),
            //            Category = Repository<Category>.Get().First(),
            //            CollectedTimeRange = Repository<CollectedTimeRange>.Get().First(),
            //            DisabilityLevel = Repository<DisabilityLevel>.Get().First(),
            //            RootGrain = Repository<Grain>.Get().First(),
            //            Level = Repository<Level>.Get().First(),
            //            IdType = Repository<RelicIdType>.Get().First(),
            //            Source = Repository<Source>.Get().First(),
            //            StoringCondition = Repository<StoringCondition>.Get().First(),
            //            WeightRange = Repository<WeightRange>.Get().First(),
            //            Unit = Repository<Unit>.Get().First(x => x.Name == "件"),
            //            TotalAmount = 100,
            //            Name = "测试文物 -- " + i.ToString(),
            //            OriginalName = "测试文物",
            //            RelicId = (i + 1000).ToString(),
            //            CollectedYearOfTime = "2001"
            //        });
            //    }
            //}
        }
    }
}
