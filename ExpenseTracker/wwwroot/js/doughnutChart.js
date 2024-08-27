Chart.register(ChartDataLabels);


if (window.chartData) {

    var labels = chartData.map(item => item.Category);
    var values = chartData.map(item => item.Sum);
    var formattedValues = chartData.map(item => item.FormattedAmount);


    //Color Selection
    const colors = ['#8BC1F7', '#BDE2B9', '#A2D9D9', '#519DE9', '#7CC674', '#73C5C5', '#06C', '#F4B678', '#8481DD', '#EC7A08', '#F0AB00', '#2A265F', '#005F60', '#F6D173']
    const backgroundColors = values.map((_, index) => getColor(index));
    function getColor(index) {
        return colors[index % colors.length];
    }


    var chartId = document.getElementById('myChart').getContext('2d');


    Chart.defaults.font.family = 'Libre Franklin';

    var myChart = new Chart(chartId, {

        type: 'doughnut',
        data: {
            labels: labels,
            datasets: [{
                data: values,
                backgroundColor: backgroundColors,
                borderColor: '#121212',
                borderWidth: 2
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            layout: {
                padding: {
                    left: 10,
                    right: 10,
                    bottom: 10,
                    top: 2


                }
            },
            plugins: {
                datalabels: {
                    color: '#FFFFFF',
                    display: true,
                    formatter: function (value, context) {
                        return context.chart.data.labels[context.dataIndex];
                    },
                    font: {
                        size: 10,
                       
                        
                    },
                    padding: {
                        top: 5,    // Add padding to improve visibility
                        bottom: 5
                    },
                    clamp: true,  // Ensures labels do not overflow the chart area
                    backgroundColor: 'rgb(106, 90, 205, 0.6)',
                    borderRadius: 4,
                    padding: {
                        top: 5,
                        bottom: 5,
                    },
                    anchor: 'center',
                    align: 'center',
                    offset: 2
                },
                title: {
                    display: true,
                    text: 'Categories',
                    color: '#9FA6B2',
                    font: {
                        size: 22,

                    }
                 
                },
                legend: {
                    display: true,
                    position: 'bottom',
                    labels: {
                        padding: 20,
                        usePointStyle: true,
                        pointStyle: 'circle',
                        font: {
                            size: 10
                        }
                    }
                },
                tooltip: {
                    callbacks: {
                        label: function (tooltipItem, data) {
                            const index = tooltipItem.dataIndex;
                            return formattedValues[index];
                        }

                    }
                },


            }
        }

    });

} else {
    console.error('chartData is not available');
}

