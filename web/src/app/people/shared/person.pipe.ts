import { Pipe, PipeTransform } from '@angular/core';
import { PersonForm } from './person-form.model';

@Pipe({
  name: 'person'
})
export class PersonPipe implements PipeTransform {

  transform(value: PersonForm, args?: any): any {
    let result: string = `${value.name} ${value.lastName}`;
    if (value.birthDate) {
      result += `, d.o.b. ${value.birthDate}`;
    }
    return result;
  }

}
