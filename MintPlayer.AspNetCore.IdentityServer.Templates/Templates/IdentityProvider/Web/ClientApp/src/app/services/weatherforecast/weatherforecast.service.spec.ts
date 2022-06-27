import { TestBed } from '@angular/core/testing';

import { WeatherforecastService } from './weatherforecast.service';

describe('WeatherforecastService', () => {
  let service: WeatherforecastService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(WeatherforecastService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
