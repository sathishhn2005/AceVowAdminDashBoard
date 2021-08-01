//[Dashboard Javascript]

//Project:	VoiceX Admin - Responsive Admin Template
//Primary use:   Used only for the main dashboard (index.html)


$(function () {
    var myChart = echarts.init(document.getElementById('patients-overview'));

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
        color: ["#38649f", "#389f99"],
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
                name: 'OP',
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
                name: 'IP',
                type: 'bar',
                data: [35, 41, 36, 26, 45, 48, 52, 53, 41, 58, 13, 123],
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
    var poleChart = echarts.init(document.getElementById('pole-chart'));
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
            x: document.getElementById('pole-chart').offsetWidth / 2,
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
        bindto: "#OPLossRatioChart",
        size: { height: 200 },
        point: { r: 3 },
        color: { pattern: ["#0bb2d4", "#17b3a3"] },
        legend: {
            show: false
        },
        data: {
            x: 'x',
            xFormat: '%Y',
            columns: [
                ['x', '2019', '2020', '2021'],
                ['data1', 0.10, 0.30, 0.15],
                ['data2', 0.20, 0.40, 0.30]
            ]
        },
        axis: {
            x: {
                type: 'timeseries',
                tick: {
                    format: '%Y'
                }
            }
        }
    });
    var t4 = c3.generate({
        bindto: "#IPLossRatioChart",
        size: { height: 200 },
        point: { r: 3 },
        color: { pattern: ["#0bb2d4", "#17b3a3"] },
        legend: {
            show: false
        },
        data: {
            columns: [
                ['data1', 0.10, 0.30, 0.15, 0.20, 0.50, 0.70],
                ['data2', 0.20, 0.40, 0.30, 0.05, 0.7, 0.6]
            ]
        }
    });
    var t5 = c3.generate({
        bindto: "#claimCostChart",
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

    // Horizental Bar Chart
    var options = {
        series: [{
            data: [1280, 1106, 1005, 987, 850, 700, 650, 500, 320, 210]
        }],
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
            categories: ['Aster al Raffah Hospital', 'Burjeel Hospital', 'Apollo Hospitals ', 'Muscat Private Hospital', 'Lifeline Hospital', 'NMC Specialty Hospital', 'Starcare Hospitals',
                'Abeer Hospital', 'Badr Al Samaa', 'Khoula Hospital'
            ],
        }
    };

    var OPHospitalsChart = new ApexCharts(document.querySelector("#topOPHospitals"), options);
    OPHospitalsChart.render();
    // Horizental Bar Chart
    var options = {
        series: [{
            data: [1350, 1306, 1205, 1087, 950, 800, 750, 600, 320, 210]
        }],
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
            categories: ['Burjeel Hospital', 'Khoula Hospital', 'Apollo Hospitals ', 'Aster al Raffah Hospital', 'Lifeline Hospital', 'NMC Specialty Hospital', 'Muscat Private Hospital',
                'Abeer Hospital', 'Badr Al Samaa',
            ],
        }
    };

    var chart = new ApexCharts(document.querySelector("#topIPHospitals"), options);
    chart.render();

    //Top 10 Diseases
    var basicpieChart = echarts.init(document.getElementById('topDiseases'));
    var option = {
        // Add title
        title: {
            text: 'Top 10 Diseases-wise Claimed Chart',
            subtext: 'Purely Fictitious',
            x: 'center'
        },

        // Add tooltip
        tooltip: {
            trigger: 'item',
            formatter: "{a} <br/>{b}: {c} ({d}%)"
        },

        // Add legend
        legend: {
            orient: 'vertical',
            x: 'left',
            data: ['Coronary Artery Disease', 'Stroke', 'Lower respiratory infections', 'Chronic', 'Lung Cancers', 'Diabetes mellitus', 'Alzheimers disease', 'Diarrheal diseases', 'Tuberculosis', 'Cirrhosis' ]
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
                dataView: {
                    show: true,
                    readOnly: false,
                    title: 'View data',
                    lang: ['View chart data', 'Close', 'Update']
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
            name: 'Marketing',
            type: 'pie',
            radius: '70%',
            center: ['50%', '57.5%'],
            data: [
                { value: 1548, name: 'Coronary Artery Disease' },
                { value: 1355, name: 'Stroke' },
                { value: 1250, name: 'Lower respiratory infections' },
                { value: 800, name: 'Chronic' },
                { value: 750, name: 'Lung Cancers' },
                { value: 700, name: 'Diabetes mellitus' },
                { value: 555, name: 'Alzheimer disease' },
                { value: 349, name: 'Diarrheal diseases' },
                { value: 300, name: 'Tuberculosis' },
                { value: 230, name: 'Cirrhosis' }
            ]
        }]
    };

    basicpieChart.setOption(option);


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






