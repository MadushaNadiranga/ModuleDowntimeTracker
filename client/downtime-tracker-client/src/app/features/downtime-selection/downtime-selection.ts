import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-downtime-selection',
  standalone: true,
  imports: [],
  templateUrl: './downtime-selection.html',
  styleUrl: './downtime-selection.scss'
})
export class DowntimeSelection {
  constructor(private router: Router) {}

  goToPlanned(): void {
    this.router.navigate(['/planned-downtime']);
  }

  goToUnplanned(): void {
    this.router.navigate(['/unplanned-downtime']);
  }
}