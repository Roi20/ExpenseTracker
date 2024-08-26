


var chartId = document.getElementById('myChart').getContext('2d');


Chart.defaults.font.family = 'Libre Franklin';

var myChart = new Chart(chartId, {

    type: 'doughnut',
    data: {
        labels: ['Data 1', 'Data 2', 'Data 3'],
        datasets: [{
            label: 'Sample Label',
            data: [100, 300, 50],
            backgroundColor: [
                'rgba(255, 99, 132, 1)',
                'rgba(54, 162, 235, 1)',
                'rgba(255, 206, 86, 1)'
            ],
            borderColor: '#121212',
            borderWidth: 5
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
            }
        }
    }
    
});
