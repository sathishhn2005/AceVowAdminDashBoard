//[Dashboard Javascript]

//Project:	VoiceX Admin - Responsive Admin Template
//Primary use:   Used only for the main dashboard (index.html)


$(function () {
    var myChart = echarts.init(document.getElementById('active-members'));

    // specify chart configuration item and data
    var option = {
        // Setup grid
        grid: {
            left: '5%',
            right: '8%',
            bottom: '3%',
            containLabel: true
        },

        // Add Tooltip
        tooltip: {
            trigger: 'axis'
        },

        legend: {
            data: ['Site A', 'Site B']
        },
        toolbox: {
            show: true,
            feature: {

                magicType: { show: true, type: ['line', 'bar'] },
                restore: { show: true },
                saveAsImage: { show: true }
            }
        },
        color: ["#38649f", "#ee1044", "#389f99"],
        calculable: true,
        xAxis: [
            {
                type: 'category',
                data: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'July', 'Aug', 'Sept', 'Oct', 'Nov', 'Dec']
            }
        ],
        yAxis: [
            {
                type: 'value'
            }
        ],
        series: [
            {
                name: 'Employee',
                type: 'bar',
                data: [76, 85, 101, 98, 87, 105, 91, 114, 94, 34, 111, 45],
                markPoint: {
                    data: [
                        { type: 'max', name: 'Max' },
                        { type: 'min', name: 'Min' }
                    ]
                },
                markLine: {
                    data: [
                        { type: 'average', name: 'Average' }
                    ]
                }
            },
            {
                name: 'Spouse',
                type: 'bar',
                data: [70, 80, 95, 90, 80, 100, 90, 110, 90, 34, 111, 44],
                markPoint: {
                    data: [
                        { name: 'The highest year', value: 182.2, xAxis: 7, yAxis: 183, symbolSize: 18 },
                        { name: 'Year minimum', value: 2.3, xAxis: 11, yAxis: 3 }
                    ]
                },
                markLine: {
                    data: [
                        { type: 'average', name: 'Average' }
                    ]
                }
            },
            {
                name: 'Child',
                type: 'bar',
                data: [20, 21, 16, 6, 35, 38, 42, 43, 31, 48, 3, 90],
                markPoint: {
                    data: [
                        { name: 'The highest year', value: 182.2, xAxis: 7, yAxis: 183, symbolSize: 18 },
                        { name: 'Year minimum', value: 2.3, xAxis: 11, yAxis: 3 }
                    ]
                },
                markLine: {
                    data: [
                        { type: 'average', name: 'Average' }
                    ]
                }
            }
        ]
    };
    // use configuration item and data specified to show chart
    myChart.setOption(option);

    // ------------------------------
    // pole chart
    // ------------------------------
    // based on prepared DOM, initialize echarts instance
    var poleChart = echarts.init(document.getElementById('pole-chart-overview'));
    // Data style
    var dataStyle = {
        normal: {
            label: { show: false },
            labelLine: { show: false }
        }
    };

    // Placeholder style
    var placeHolderStyle = {
        normal: {
            color: 'rgba(0,0,0,0)',
            label: { show: false },
            labelLine: { show: false }
        },
        emphasis: {
            color: 'rgba(0,0,0,0)'
        }
    };
    var option = {
        title: {
            text: 'Patients In',
            subtext: 'Daily Data',
            x: 'center',
            y: 'center',
            itemGap: 10,
            textStyle: {
                color: 'rgba(30,144,255,0.8)',
                fontSize: 19,
                fontWeight: '500'
            }
        },

        // Add tooltip
        tooltip: {
            show: true,
            formatter: "{a} <br/>{b}: {c} ({d}%)"
        },

        // Add legend
        legend: {
            orient: 'vertical',
            x: document.getElementById('pole-chart-overview').offsetWidth / 2,
            y: 30,
            x: '55%',
            itemGap: 15,
            data: ['ICU', 'OPD', 'Emergency']
        },

        // Add custom colors
        color: ['#689f38', '#38649f', '#ff8f00'],

        // Add series
        series: [
            {
                name: '1',
                type: 'pie',
                clockWise: false,
                radius: ['75%', '90%'],
                itemStyle: dataStyle,
                data: [
                    {
                        value: 60,
                        name: 'ICU'
                    },
                    {
                        value: 40,
                        name: 'invisible',
                        itemStyle: placeHolderStyle
                    }
                ]
            },

            {
                name: '2',
                type: 'pie',
                clockWise: false,
                radius: ['60%', '75%'],
                itemStyle: dataStyle,
                data: [
                    {
                        value: 30,
                        name: 'OPD'
                    },
                    {
                        value: 70,
                        name: 'invisible',
                        itemStyle: placeHolderStyle
                    }
                ]
            },

            {
                name: '3',
                type: 'pie',
                clockWise: false,
                radius: ['45%', '60%'],
                itemStyle: dataStyle,
                data: [
                    {
                        value: 10,
                        name: 'Emergency'
                    },
                    {
                        value: 90,
                        name: 'invisible',
                        itemStyle: placeHolderStyle
                    }
                ]
            }
        ]
    };
    poleChart.setOption(option);

    var t3 = c3.generate({
        bindto: "#OPIPLossRatioBIChart",
        size: { height: 200 },
        point: { r: 3 },
        color: { pattern: ["#38649f", "#689f38"] },
        legend: {
            show: false
        },
        data: {
            x: 'x',
            xFormat: '%Y',
            columns: [
                ['x', '2019', '2020', '2021'],
                ['OP', 0.10, 0.40, 0.15],
                ['IP', 0.20, 0.30, 0.30]
            ]
        },
        axis: {
            x: {
                type: 'category ',
                tick: {
                    format: '%Y'
                }
            }
        }
    });
    var t4 = c3.generate({
        bindto: "#BenefitwiseRatio",
        size: { height: 200 },
        point: { r: 3 },
        color: { pattern: ["#38649f", "#689f38"] },
        legend: {
            show: false
        },
        data: {
            columns: [
                ['data1', 0.10, 0.30, 0.15, 0.20, 0.50, 0.70],
                ['data2', 0.20, 0.40, 0.30, 0.05, 0.7, 0.6]
            ]
        },
        axis: {
            x: {
                type: "category"
            }
        }
    });
    var t5 = c3.generate({
        bindto: "#avgClaimCostChart",
        size: { height: 200 },
        point: { r: 4 },
        color: { pattern: ["#3e8ef7", "#ff4c52"] },
        legend: {
            show: false
        },
        data: {
            columns: [
                ['data1', 300, 350, 300, 0, 0, 0],
                ['data2', 130, 100, 140, 200, 150, 50]

            ],
            types: { data1: "area", data2: "area-spline" }
        },
        grid: { y: { show: !0 } }
    });

    var basicpieChart = echarts.init(document.getElementById('industryWiseLoss'));
    var option = {
        // Add title
        title: {
            text: 'Industry-wise Loss Ratio',
            x: 'center'
        },

        // Add tooltip
        tooltip: {
            trigger: 'item',
            formatter: "{b} <br/>{a}: {c}%"
        },

        // Add legend
        legend: {
            orient: 'vertical',
            x: 'left',
            y: 'center',
            data: ['Oil and Petroleum', 'Contracting Companies', 'Cement', 'Aluminium Spellers', 'Poultry Industry', 'Natural Gas', 'Agriculture', 'Optic fiber', 'Construction', 'Copper']
        },

        // Add custom colors
        color: ['#689f38', '#38649f', '#389f99', '#ee1044', '#ff8f00'],

        // Display toolbox
        toolbox: {
            show: true,
            orient: 'vertical',
            feature: {
                mark: {
                    show: true,
                    title: {
                        mark: 'Markline switch',
                        markUndo: 'Undo markline',
                        markClear: 'Clear markline'
                    }
                },
                magicType: {
                    show: true,
                    title: {
                        pie: 'Switch to pies',
                        funnel: 'Switch to funnel',
                    },
                    type: ['pie', 'funnel'],
                    option: {
                        funnel: {
                            x: '25%',
                            y: '20%',
                            width: '50%',
                            height: '70%',
                            funnelAlign: 'left',
                            max: 1548
                        }
                    }
                },
                restore: {
                    show: true,
                    title: 'Restore'
                },
                saveAsImage: {
                    show: true,
                    title: 'Same as image',
                    lang: ['Save']
                }
            }
        },

        // Enable drag recalculate
        calculable: true,

        // Add series
        series: [{
            name: 'Loss Ratio',
            type: 'pie',
            radius: '70%',
            center: ['50%', '55%'],
            data: [
                { value: 13.9, name: 'Oil and Petroleum' },
                { value: 8.3, name: 'Contracting Companies' },
                { value: 7.0, name: 'Cement' },
                { value: 4.07, name: 'Aluminium Spellers' },
                { value: 4.05, name: 'Poultry Industry' },
                { value: 3.8, name: 'Natural Gas' },
                { value: 3.2, name: 'Agriculture' },
                { value: 3.0, name: 'Optic fiber' },
                { value: 2.9, name: 'Construction' },
                { value: 2.5, name: 'Copper' }
            ]
        }]
    };

    basicpieChart.setOption(option);

    var locationpieChart = echarts.init(document.getElementById('locationWiseLoss'));
    var option = {
        // Add title
        title: {
            text: 'Location-wise Loss Ratio',
            x: 'center'
        },

        // Add tooltip
        tooltip: {
            trigger: 'item',
            formatter: "{b} <br/>{a}: {c}%"
        },

        // Add legend
        legend: {
            orient: 'horizontal',
            x: 'center',
            y: 'bottom',
            data: ['Muscat', 'Salalah', 'Nizwa', 'Sohar', 'Sur', 'Bahal', 'Rustaq', 'Seeb', 'Mutrah', 'Bawshar']
        },

        // Add custom colors
        color: ['#689f38', '#38649f', '#389f99', '#ee1044', '#ff8f00'],

        // Display toolbox
        toolbox: {
            show: true,
            orient: 'vertical',
            feature: {
                mark: {
                    show: true,
                    title: {
                        mark: 'Markline switch',
                        markUndo: 'Undo markline',
                        markClear: 'Clear markline'
                    }
                },
                magicType: {
                    show: true,
                    title: {
                        pie: 'Switch to pies',
                        funnel: 'Switch to funnel',
                    },
                    type: ['pie', 'funnel'],
                    option: {
                        funnel: {
                            x: '25%',
                            y: '20%',
                            width: '50%',
                            height: '70%',
                            funnelAlign: 'left',
                            max: 1548
                        }
                    }
                },
                restore: {
                    show: true,
                    title: 'Restore'
                },
                saveAsImage: {
                    show: true,
                    title: 'Same as image',
                    lang: ['Save']
                }
            }
        },

        // Enable drag recalculate
        calculable: true,

        // Add series
        series: [{
            name: 'Loss Ratio',
            type: 'pie',
            radius: ['30%', '50%'],
            center: ['50%', '45%'],
            data: [
                { value: 10.9, name: 'Muscat' },
                { value: 8.3, name: 'Salalah' },
                { value: 7.0, name: 'Nizwa' },
                { value: 4.07, name: 'Sohar' },
                { value: 4.05, name: 'Sur' },
                { value: 3.8, name: 'Bahal' },
                { value: 3.2, name: 'Rustaq' },
                { value: 3.0, name: 'Seeb' },
                { value: 2.9, name: 'Mutrah' },
                { value: 2.5, name: 'Bawshar' }
            ]
        }]
    };
    locationpieChart.setOption(option);

    var options = {
        series: [{
            data: [1280, 1106, 1005, 987, 850, 700, 650, 500, 320, 210]
        }],
        colors: ['#38649f'],
        chart: {
            type: 'bar',
            height: 350
        },
        plotOptions: {
            bar: {
                horizontal: true,
            }
        },
        dataLabels: {
            enabled: false
        },
        xaxis: {
            categories: ['Amur Humaid Juma', 'Burjeel Hospital', 'Gilbert Munyoki', 'Vicky Lobin', 'Saharat Sanpanorasate', 'Benjamin Saju', 'Shiva Raj Subedi', 'Vikash Baduwal', 'Ramesh Kumar Bista', 'Ram Bahadur Darji'
            ],
        }
    };

    var OPHospitalsChart = new ApexCharts(document.querySelector("#topSpendingMembers"), options);
    OPHospitalsChart.render();

    $('.inner-user-div').slimScroll({
        height: '370px'
    });

    //dashboard_daterangepicker

    if (0 !== $("#dashboard_daterangepicker").length) {
        var n = $("#dashboard_daterangepicker"),
            e = moment(),
            t = moment();
        n.daterangepicker({
            startDate: e, endDate: t, opens: "left", ranges: {
                Today: [moment(), moment()], Yesterday: [moment().subtract(1, "days"), moment().subtract(1, "days")], "Last 7 Days": [moment().subtract(6, "days"), moment()], "Last 30 Days": [moment().subtract(29, "days"), moment()], "This Month": [moment().startOf("month"), moment().endOf("month")], "Last Month": [moment().subtract(1, "month").startOf("month"), moment().subtract(1, "month").endOf("month")]
            }
        }
            , a),
            a(e, t, "")
    }
    function a(e, t, a) {
        var r = "",
            o = "";
        t - e < 100 || "Today" == a ? (r = "Today:", o = e.format("MMM D")) : "Yesterday" == a ? (r = "Yesterday:", o = e.format("MMM D")) : o = e.format("MMM D") + " - " + t.format("MMM D"), n.find(".subheader_daterange-date").html(o), n.find(".subheader_daterange-title").html(r)
    }
}); // End of use strict






