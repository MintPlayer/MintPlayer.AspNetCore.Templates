import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WeatherforecastsComponent } from './weatherforecasts.component';

describe('WeatherforecastsComponent', () => {
	let component: WeatherforecastsComponent;
	let fixture: ComponentFixture<WeatherforecastsComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			declarations: [WeatherforecastsComponent]
		})
			.compileComponents();

		fixture = TestBed.createComponent(WeatherforecastsComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});
});
