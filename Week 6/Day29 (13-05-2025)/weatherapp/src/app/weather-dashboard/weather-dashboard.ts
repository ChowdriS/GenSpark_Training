import { Component, OnInit, effect, signal } from '@angular/core';
import { WeatherService } from '../Service/WeatherService';
import { CitySearchComponent } from "../city-search/city-search";
import { WeatherCardComponent } from "../weather-card/weather-card";
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-weather-dashboard',
  templateUrl: './weather-dashboard.html',
  standalone: true,
  imports: [CitySearchComponent, WeatherCardComponent, NgIf]
})
export class WeatherDashboardComponent implements OnInit {
  weatherData = signal<any>(null);
  city = '';

  constructor(private weatherService: WeatherService) {}

  ngOnInit() {
    this.weatherService.city$.subscribe(city => {
      this.city = city;
      if (city) {
        this.weatherService.getWeather(city).subscribe(data => this.weatherData.set(data));
      }
    });
  }
}