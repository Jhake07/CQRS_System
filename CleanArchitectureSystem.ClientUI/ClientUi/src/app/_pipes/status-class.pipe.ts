import { Pipe, PipeTransform } from '@angular/core';
import { Status } from '../_enums/status.enum';

@Pipe({
  name: 'statusClass',
  standalone: true,
})
export class StatusClassPipe implements PipeTransform {
  transform(status: string): string {
    switch (status) {
      case Status.Open:
        return 'bg-info text-dark';
      case Status.InProgress:
        return 'bg-warning text-dark';
      case Status.Completed:
        return 'bg-success';
      case Status.Cancelled:
        return 'bg-danger';
      default:
        return 'bg-secondary';
    }
  }
}
