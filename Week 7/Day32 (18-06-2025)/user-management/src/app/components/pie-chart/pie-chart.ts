import { Component, Input, OnInit } from '@angular/core';
import Chart from 'chart.js/auto';

@Component({
  selector: 'app-pie-chart',
  imports: [],
  templateUrl: './pie-chart.html',
  styleUrls: ['./pie-chart.css']
})
export class PieChart implements OnInit {
  @Input() chartData : any;
  chart : any;
  ngOnInit() {
    console.log(this.chartData)
    
  }

  createChart(maleFemaleCnt:number[]){
    this.chart = new Chart("MyChart", {
      type: 'pie', 
      data: {
        labels: ['Male','Female'],
           datasets: [{
              label: 'Male Female Ratio',
              data: maleFemaleCnt,
              backgroundColor: [
                'blue',            
                'pink'
              ],
              hoverOffset: 4
            }],
      },
      options: {
        aspectRatio:2.5
      }

    });
  }

}
