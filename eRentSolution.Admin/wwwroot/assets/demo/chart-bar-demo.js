// Set new default font family and font color to mimic Bootstrap's default styling
Chart.defaults.global.defaultFontFamily = '-apple-system,system-ui,BlinkMacSystemFont,"Segoe UI",Roboto,"Helvetica Neue",Arial,sans-serif';
Chart.defaults.global.defaultFontColor = '#292b2c';

// Bar Chart Example
var ctx = document.getElementById("myBarChart");
var input_name = document.querySelectorAll(".input-name")
var input_view = document.querySelectorAll(".input-viewcount-user")
var NameValue = new Array();
var ViewUserValue = new Array();
for (var i = 0; i < input_name.length; i++) {
    NameValue[i] = input_name[i].value;
    ViewUserValue[i] = input_view[i].value;
}
console.log(NameValue)
var max_value = ViewUserValue[0];
for (var i = 0; i < ViewUserValue.length; i++) {
    if (max_value < ViewUserValue[i])
        max_value = ViewUserValue[i];
}
let max1 = Number(max_value) * 2;
console.log(max1)
var myLineChart = new Chart(ctx, {
  type: 'bar',
  data: {
      labels: NameValue,
    datasets: [{
      label: "Số sản phẩm",
     
        data: ViewUserValue,
        backgroundColor: [
            'rgba(255, 99, 132, 0.6)',
            'rgba(54, 162, 235, 0.6)',
            'rgba(255, 206, 86, 0.6)',
            'rgba(75, 192, 192, 0.6)',
            'rgba(153, 102, 255, 0.6)',
            'rgba(255, 159, 64, 0.6)',
            'rgba(255, 99, 132, 0.6)'
        ],
        borderWidth: 1,
        borderColor: '#777',
        hoverBorderWidth: 3,
        hoverBorderColor: '#000'
    }],
  },
  options: {
    scales: {
      xAxes: [{
        time: {
          unit: 'month'
        },
        gridLines: {
          display: false
        },
        ticks: {
          maxTicksLimit: 6
        }
      }],
      yAxes: [{
        ticks: {
          min: 0,
              max: max1,
          maxTicksLimit: 5
        },
        gridLines: {
          display: true
        }
      }],
    },
    legend: {
      display: false
    }
  }
});
