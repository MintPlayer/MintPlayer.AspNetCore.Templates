import { Injectable } from '@angular/core';
import { BaseUrlService } from '@mintplayer/ng-base-url';

@Injectable({
  providedIn: 'root'
})
export class WeatherforecastService {

  constructor(baseUrlService: BaseUrlService) {
    this.baseUrl = baseUrlService.getBaseUrl({ dropScheme: true });
  }

  private baseUrl: string | null;

}
