import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ExternalLoginButtonComponent } from './external-login-button.component';

describe('ExternalLoginButtonComponent', () => {
	let component: ExternalLoginButtonComponent;
	let fixture: ComponentFixture<ExternalLoginButtonComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			declarations: [ExternalLoginButtonComponent]
		})
			.compileComponents();

		fixture = TestBed.createComponent(ExternalLoginButtonComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});
});
