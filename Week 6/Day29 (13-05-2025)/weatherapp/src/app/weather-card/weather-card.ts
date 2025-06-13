import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-weather-card',
  templateUrl: './weather-card.html',
  standalone: true,
})
export class WeatherCardComponent {
  @Input() data: any;
}
