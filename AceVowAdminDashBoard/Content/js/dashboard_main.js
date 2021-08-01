//[Dashboard Javascript]

//Project:	VoiceX Admin - Responsive Admin Template
//Primary use:   Used only for the main dashboard (index.html)


$(function () {
    var chartData = [];
    var chartDataLineBar = [];
    var UWTotal = [];
    var UWPending = [];
    var UWApproved = [];
    var UWRejected = [];
    'use strict';
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: "GetLineChart",
        data: "{}",
        dataType: "json",
        success: function (Result) {

            chartDataLineBar = Result.list;

            jQuery.each(chartDataLineBar, function (key, value) {

                UWPending.push(value.a);
                UWApproved.push(value.b);
                UWRejected.push(value.c);
                UWTotal.push(value.d);


            });
            //jQuery.each(chartDataLineBar, function (key, value) {

            //	UnderWriterResultTPL.push(value.b);

            //});

        },
        error: function (Result) {
            //alert("Error");
        }
    });

    var options = {
        chart: {
            height: 350,
            type: 'line',
        },
        plotOptions: {
            bar: {
                horizontal: false,
                columnWidth: '70%',
                endingShape: 'rounded'
            },
        },
        dataLabels: {
            enabled: false
        },
        colors: ["#4974e0", '#e2bb33', '#3db76b', '#e83a75'],
        stroke: {
            width: 3,
            curve: 'smooth',
        },

        series: [{
            name: 'No.of Policies',
            data: UWTotal
        }, {
            name: 'No.of Quotations',
            data: UWPending
        }, {
            name: 'Policy Issued',
            data: UWApproved
        }, {
            name: 'Policy Lost',
            data: UWRejected
        }],
        xaxis: {
            categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
        },
        yaxis: {
            title: {
                text: 'Policies Count'
            }
        },
        fill: {
            opacity: 1

        },
        legend: {
            position: 'top',
            horizontalAlign: 'left',
        },
        tooltip: {
            y: {
                formatter: function (val) {
                    return val
                }
            }
        }
    }

    var chart1 = new ApexCharts(
        document.querySelector("#uni-earning"),
        options
    );
    var UnderWriterResultTPE = [];
    var UnderWriterResultTPL = [];
    
    
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: "GetRevenueByYear",
        data: "{}",
        dataType: "json",
        success: function (Result) {

            chartData = Result.list;

            jQuery.each(chartData, function (key, value) {

                UnderWriterResultTPE.push(value.a);
                // console.log(UnderWriterResult);
                //write some code here to build the table? I'm just guessing
            });
            jQuery.each(chartData, function (key, value) {

                UnderWriterResultTPL.push(value.b);
                //   console.log(UnderWriterResult);
                //write some code here to build the table? I'm just guessing
            });

        },
        error: function (Result) {
           // alert("Error");
        }
    });
    var options = {
        chart: {
            height: 470,
            type: 'bar',
            stacked: true,
            shadow: {
                enabled: true,
                color: '#000',
                top: 18,
                left: 7,
                blur: 10,
                opacity: 1
            },
            toolbar: {
                show: false
            }
        },
        colors: ['#ee1044', '#38649f'],
        dataLabels: {
            enabled: true,
        },
        stroke: {
            curve: 'smooth'
        },

        series: [{
            name: "Total Premium Earned",
            data: UnderWriterResultTPE
        },
        {
            name: "Total Premium Lost",
            data: UnderWriterResultTPL
        }
        ],
        // series: chartData,
        grid: {
            borderColor: '#e7e7e7',
            row: {
                colors: ['#f3f3f3', 'transparent'], // takes an array which will be repeated on columns
                opacity: 0.5
            },
        },
        markers: {

            size: 4
        },
        xaxis: {
            categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul'],
            title: {
                text: 'Month'
            }
        },
        yaxis: {
            title: {
                text: 'Amount'
            },
        },
        legend: {
            position: 'top',
            horizontalAlign: 'right',
            floating: true,
        }
    }
    var chart2 = new ApexCharts(
        document.querySelector("#uni-attendance"),
        options
    );
    


    window.Apex = {
        stroke: {
            width: 3
        },
        markers: {
            size: 0
        },
        tooltip: {
            theme: 'dark',
        }
    };

    var randomizeArray = function (arg) {
        var array = arg.slice();
        var currentIndex = array.length,
            temporaryValue, randomIndex;

        while (0 !== currentIndex) {

            randomIndex = Math.floor(Math.random() * currentIndex);
            currentIndex -= 1;

            temporaryValue = array[currentIndex];
            array[currentIndex] = array[randomIndex];
            array[randomIndex] = temporaryValue;
        }

        return array;
    }



    ///**************** PIE CHART *******************/
    //var piedata = [
    //	{ label: "By Branches", data: [[1, 40]], color: '#38649f' },
    //	{ label: "By DirectSales", data: [[1, 50]], color: '#389f99' },
    //	{ label: "By Broker", data: [[1, 60]], color: '#689f38' },
    //	{ label: "By Agent", data: [[1, 60]], color: '#ff8f00' }
    //];

    //   $.plot('#flotPie2', piedata, {
    //     series: {
    //       pie: {
    //         show: true,
    //         radius: 1,
    //         innerRadius: 0.5,
    //         label: {
    //           show: true,
    //           radius: 2/3,
    //           formatter: labelFormatter,
    //           threshold: 0.1
    //         }
    //       }
    //     },
    //     grid: {
    //       hoverable: true,
    //       clickable: true
    //     }
    //   });

    //   function labelFormatter(label, series) {
    //	  return "<div style='font-size:8pt; text-align:center; padding:2px; color:white;'>" + label + "<br/>" + Math.round(series.percent) + "%</div>";
    //  }

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
    setTimeout(function () {
        chart1.render();
        chart2.render();
    }, 1000);


}); // End of use strict






