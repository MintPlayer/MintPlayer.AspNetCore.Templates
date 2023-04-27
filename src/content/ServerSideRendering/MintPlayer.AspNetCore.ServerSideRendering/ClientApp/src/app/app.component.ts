import { Component } from '@angular/core';
import { Color } from '@mintplayer/ng-bootstrap';

@Component({
	selector: 'app-root',
	templateUrl: './app.component.html',
	styleUrls: ['./app.component.scss']
})
export class AppComponent {
	title = 'ClientApp';
	colors = Color;
}
