using IMS.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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

        private void InitializeData()
        {
            if (Repository<Age>.Get().Count == 0)
            {
                Repository<Age>.Add(new Age()
                {
                    Name = "中国历史学年代",
                    Children = new List<Age>()
                    {
                        new Age(){ Name = "夏(约前2070~前1600)" },
                        new Age(){ Name = "商(前1600~前1046)" },
                        new Age(){ Name = "周",
                            Children = new List<Age>()
                            {
                                new Age() { Name = "西周(前1046~前771)" },
                                new Age() { Name = "东周(前770~前256)" },
                                new Age() { Name = "春秋时代(前770~前476)" },
                                new Age() { Name = "战国时代(前475~前221)" }
                            }
                        },
                        new Age(){ Name = "秦(前221~前206)" },
                        new Age(){ Name = "汉",
                            Children = new List<Age>()
                            {
                                new Age(){ Name ="西汉(前206~公元25)" },
                                new Age(){ Name ="东汉(25~220)" }
                            }
                        },
                        new Age(){ Name = "三国",
                            Children = new List<Age>()
                            {
                                new Age(){ Name ="三国·魏(220~265)" },
                                new Age(){ Name ="三国·蜀(221~263)" },
                                new Age(){ Name ="三国·吴(222~280)" }
                            }
                        },
                        new Age(){ Name = "西晋(265~317)" },
                        new Age(){ Name = "东晋十六国",
                            Children = new List<Age>()
                            {
                                new Age(){ Name ="东晋(317~420)" },
                                new Age(){ Name ="十六国(304~439)" }
                            }
                        },
                        new Age(){ Name = "南北朝",
                            Children = new List<Age>()
                            {
                                new Age(){ Name ="南朝",
                                    Children = new List<Age>()
                                    {
                                        new Age(){ Name ="南朝·宋(420~479)" },
                                        new Age(){ Name ="南朝·齐(479~502)" },
                                        new Age(){ Name ="南朝·梁(502~557)" },
                                        new Age(){ Name ="南朝·陈(557~589)" },
                                    }
                                },
                                new Age(){ Name ="北朝",
                                    Children = new List<Age>()
                                    {
                                        new Age(){ Name = "北魏(386~534)" },
                                        new Age(){ Name = "东魏(534~550)" },
                                        new Age(){ Name = "北齐(550~577)" },
                                        new Age(){ Name = "西魏(535~556)" },
                                        new Age(){ Name = "北周(557~581)" }
                                    }
                                }
                            }
                        },
                        new Age(){ Name = "隋(581~618)" },
                        new Age(){ Name = "唐(618~907)" },
                        new Age(){ Name = "五代十国",
                            Children = new List<Age>()
                            {
                                new Age(){ Name ="五代·后梁(907~923)" },
                                new Age(){ Name ="五代·后唐(923~936)" },
                                new Age(){ Name ="五代·后晋(936~947)" },
                                new Age(){ Name ="五代·后汉(947~950)" },
                                new Age(){ Name ="五代•后周(951~960)" },
                                new Age(){ Name ="十国(902~979)" }
                            }
                        },
                        new Age(){ Name = "宋",
                            Children = new List<Age>()
                            {
                                new Age(){ Name ="北宋(960~1127)" },
                                new Age(){ Name ="南宋(1127~1279)" }
                            }
                        },
                        new Age(){ Name = "辽(907~1125)" },
                        new Age(){ Name = "西夏(1038~1227)" },
                        new Age(){ Name = "金(1115~1234)" },
                        new Age(){ Name = "元(1206~1368)" },
                        new Age(){ Name = "明(1368~1644)" },
                        new Age(){ Name = "清(1616~1911)" },
                        new Age(){ Name = "中华民国(1912~1949)" },
                        new Age(){ Name = "中华人民共和国(1949年10月1日成立)" },
                    }
                });
            }
            if (Repository<RelicIdType>.Get().Count == 0)
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
            if (Repository<Category>.Get().Count == 0)
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
            if (Repository<Grain>.Get().Count == 0)
            {
                new List<Grain>()
                {
                    new Grain(){ Name = "单一质地",
                        Children = new List<Grain>()
                        {
                            new Grain(){ Name = "有机质",
                                Children = new List<Grain>()
                                {
                                    new Grain(){ Name = "木" },
                                    new Grain(){ Name = "竹" },
                                    new Grain(){ Name = "纸" },
                                    new Grain(){ Name = "毛" },
                                    new Grain(){ Name = "丝" },
                                    new Grain(){ Name = "皮革" },
                                    new Grain(){ Name = "骨角牙" },
                                    new Grain(){ Name = "棉麻纤维" },
                                    new Grain(){ Name = "其他植物质" },
                                    new Grain(){ Name = "其他动物质" },
                                    new Grain(){ Name = "其他有机质" }
                                }
                            },
                            new Grain(){ Name = "无机质",
                                Children = new List<Grain>()
                                {
                                    new Grain(){ Name = "石" },
                                    new Grain(){ Name = "瓷" },
                                    new Grain(){ Name = "砖瓦" },
                                    new Grain(){ Name = "泥" },
                                    new Grain(){ Name = "陶" },
                                    new Grain(){ Name = "玻璃" },
                                    new Grain(){ Name = "铁" },
                                    new Grain(){ Name = "铜" },
                                    new Grain(){ Name = "宝玉石" },
                                    new Grain(){ Name = "金" },
                                    new Grain(){ Name = "银" },
                                    new Grain(){ Name = "其他金属" },
                                    new Grain(){ Name = "其他无机质" }
                                }
                            },
                        }
                    },
                    new Grain(){ Name = "复合或组合质地",
                        Children = new List<Grain>()
                        {
                            new Grain(){ Name = "有机复合或组合" },
                            new Grain(){ Name = "无机复合或组合" },
                            new Grain(){ Name = "有机无机复合或组合" }
                        }
                    },
                }.ForEach(p => Repository<Grain>.Add(p));
            }
            if (Repository<Level>.Get().Count == 0)
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
            if (Repository<Source>.Get().Count == 0)
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
            if (Repository<DisabilityLevel>.Get().Count == 0)
            {
                new List<DisabilityLevel>()
                {
                    new DisabilityLevel(){ Name = "完整" },
                    new DisabilityLevel(){ Name = "基本完整" },
                    new DisabilityLevel(){ Name = "残缺" },
                    new DisabilityLevel(){ Name = "严重残缺(含缺失部件)" }
                }.ForEach(p => Repository<DisabilityLevel>.Add(p));
            }
            if (Repository<StoringCondition>.Get().Count == 0)
            {
                new List<StoringCondition>()
                {
                    new StoringCondition(){ Name = "状态稳定，不需修复" },
                    new StoringCondition(){ Name = "部分损腐，需要修复" },
                    new StoringCondition(){ Name = "腐蚀损毁严重，急需修复" },
                    new StoringCondition(){ Name = "已修复" }
                }.ForEach(p => Repository<StoringCondition>.Add(p));
            }
            if (Repository<CollectedTimeRange>.Get().Count == 0)
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
            if (Repository<WeightRange>.Get().Count == 0)
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
            if (Repository<Relic>.Get().Count == 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    Repository<Relic>.Add(new Relic()
                    {
                        RootAge = Repository<Age>.Get().First(),
                        Amount = 100,
                        Name = "测试文物 -- " + i.ToString(),
                        Category = Repository<Category>.Get().First(),
                        IdType = Repository<RelicIdType>.Get().First(),
                        Level = Repository<Level>.Get().First(),
                        OriginalName = "测试文物",
                        RelicId = (i + 1000).ToString(),
                        Source = Repository<Source>.Get().First(),
                        CollectedTimeRange = Repository<CollectedTimeRange>.Get().First(),
                        CollectedYearOfTime = "2001",
                        DisabilityLevel = Repository<DisabilityLevel>.Get().First(),
                        RootGrain = Repository<Grain>.Get().First(),
                        WeightRange = Repository<WeightRange>.Get().First(),
                        StoringCondition = Repository<StoringCondition>.Get().First()
                    });
                }

            }
        }
    }
}
