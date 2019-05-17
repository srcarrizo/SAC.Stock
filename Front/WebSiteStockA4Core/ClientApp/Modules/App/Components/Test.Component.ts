import { Component, Inject } from '@angular/core';

@Component({
  selector: 'Test',
  //templateUrl:
  template: '<h3>{{ title }}</h3>',
  //styleUrls:
})

export class TestComponent {
  constructor(@Inject('Configuration') private Config) { }

  title() {
    return 'titulo' + this.Config;
  }

  titles = this.title();
}