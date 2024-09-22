Chart.register(ChartDataLabels);

console.log(LineChartData);

var label = LineChartData.map(x => x.NumberOfDays);
var Income = LineChartData.map(x => x.Income);
var Expense = LineChartData.map(x => x.Expense);



var formatter = new Intl.NumberFormat('en-PH', {
    style: 'currency',
    currency: 'PHP'
    
});

var formattedIncome = Income.map(value => formatter.format(value));
var formattedExpense = Expense.map(value => formatter.format(value));



var ctx = document.getElementById('myLineChart').getContext('2d');

var myLineChart = new Chart(ctx, {
    type: 'line',
    data: {
        labels: label,
        datasets: [
            {
                label: 'Income',
                data: Income,
                borderColor: '#4CB140',
                backgroundColor: '#4CB140',
                borderWidth: 2,
                fill: false,
                lineTension: 0,
                pointRadius: 1
            },
            {
                label: 'Expense',
                data: Expense,
                borderColor: '#C9190B',
                backgroundColor: '#C9190B',
                borderWidth: 2,
                fill: false, 
                lineTension: 0,
                pointRadius: 1,

            }
        ]
    },
    options: {
        responsive: true,
        maintainAspectRatio: false,
        layout: {
            padding: {
                left: 0,
                right: 0,
                bottom: 0,
                top: 0
            }
        },

        plugins: {
            legend: {
                position: 'bottom', 
                labels: {
                    font: {
                        size: '10',
                    },
                    usePointStyle: true,
                    pointStyle: 'star',
                    color: '#FFFFFF',
                    
                },
               
            },
            title: {
                display: true,
                text: 'Income vs Expense',
                color: '#FFFFFF',
                padding: {
                    bottom: 30,
                    top: 10
                },
                font: {
                    size: 18
                },
            },

            datalabels: {
                color: '#FFFFFF',
                align: 'end',  // Aligns the label towards the end of the data point
                anchor: 'end', // Anchors the label to the end of the data point
                font: {
                    size: 8,  // Adjust the font size to align better
                },
                padding: {
                    top: 0,
                    bottom: 0
                },
                display: false,
                formatter: function (value) {
                    return formatter.format(value)
                },
            }
        },
        scales: {
            y: {
                
                beginAtZero: false,
                position: 'left', // Align y-axis labels on the left side
                ticks: {
                    padding: 5, // Add padding between ticks and labels
                    font: {
                        size: 10,  // Adjust the font size
                    },
                    color: '#FFFFFF'
                },
                grid: {

                    drawOnChartArea: true,
                    drawTicks: false,
                    display: true,
                    borderWidth: 1,
                    color: 'rgba(255, 255, 255, 0.1)' 
                },
                border: {
                    dash: [1,2]
                }
            },
            x: {
                position: 'bottom', // Align x-axis labels at the bottom
                ticks: {
                    padding: 10, // Add padding between ticks and labels
                    font: {
                        size: 10  // Adjust the font size
                    },
                    maxRotation: 0,  // Prevent label rotation
                    minRotation: 0,  // Ensure labels are horizontal
                                   
                    color: '#FFFFFF'
                },
                grid: {
                    drawOnChartArea: false,
                    drawTicks: false,
                    display: true,
                    color: 'rgba(255, 255, 255, 0.2)', 
                    
                }
            }
        },
    }
});