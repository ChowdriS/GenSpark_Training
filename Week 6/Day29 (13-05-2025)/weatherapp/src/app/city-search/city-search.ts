import { Component } from '@angular/core';
import { WeatherService } from '../Service/WeatherService';
import { FormsModule } from '@angular/forms';

@Component({
 selector: 'app-city-search',
  templateUrl: './city-search.html',
  standalone: true,
  imports: [FormsModule] 
})
export class CitySearchComponent {
  city = '';

  constructor(private weatherService: WeatherService) {}

  search() {
    if (this.city.trim()) {
      this.weatherService.setCity(this.city.trim());
    }
  }
}