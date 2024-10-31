


var Labels = modeData.map(m => m.Month);
var modeIncome = modeData.map(item => item.ModeIncome);
var modeExpense = modeData.map(item => item.ModeExpense);


var ctx = document.getElementById('adminModeChart').getContext('2d');

var myChart = new Chart(ctx, {
    type: 'bar',
    data: {
        labels: Labels, 
        datasets: [
            {
                label: 'Mode Income',
                backgroundColor: 'rgba(75, 192, 192, 0.6)',
                borderColor: 'rgba(75, 192, 192, 1)',
                borderWidth: 2,
                hoverBackgroundColor: 'rgba(75, 192, 192, 0.8)',
                hoverBorderColor: 'rgba(75, 192, 192, 1)',
                data: modeIncome
            },
            {
                label: 'Mode Expense',
                backgroundColor: 'rgba(255, 99, 132, 0.6)',
                borderColor: 'rgba(255, 99, 132, 1)',
                borderWidth: 2,
                hoverBackgroundColor: 'rgba(255, 99, 132, 0.8)',
                hoverBorderColor: 'rgba(255, 99, 132, 1)',
                data: modeExpense
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
                        size: 10,
                    },
                    usePointStyle: true,
                    pointStyle: 'star',
                    color: '#FFFFFF',
                },
            },
            title: {
                display: false,
                text: 'Financial Trends',
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
                align: 'end',
                anchor: 'end',
                font: {
                    size: 8,
                },
                padding: {
                    top: 0,
                    bottom: 0
                },
                display: false,
                formatter: function (value) {
                    return formatter.format(value);
                },
            }
        },
        scales: {
            y: {
                beginAtZero: true,
                position: 'left',
                ticks: {
                    padding: 5,
                    font: {
                        size: 10,
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
                    dash: [1, 2]
                }
            },
            x: {
                position: 'bottom',
                ticks: {
                    padding: 10,
                    font: {
                        size: 10,
                    },
                    maxRotation: 0,
                    minRotation: 0,
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
