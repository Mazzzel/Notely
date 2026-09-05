import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { TopBarComponent } from '../../shared/top-bar/top-bar';

@Component({
  selector: 'app-shell',
  imports: [RouterOutlet, TopBarComponent],
  template: `
    <app-top-bar></app-top-bar>
    <router-outlet></router-outlet>
    <footer class="note">Les données sont enregistrées automatiquement.</footer>
  `,
  styles: `
    .note {
      max-width: 1080px;
      margin: 40px auto 0;
      padding: 0 24px;
      color: var(--ink-soft);
      font-size: 12px;
      font-family: 'IBM Plex Mono', monospace;
    }
  `
})
export class ShellComponent {}
