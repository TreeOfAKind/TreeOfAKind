import { Pipe, PipeTransform } from '@angular/core';
import { PersonForm } from './person-form.model';

@Pipe({
  name: 'person'
})
export class PersonPipe implements PipeTransform {

  transform(value: PersonForm, args?: any): any {
    return `${value.name} ${value.lastName}, d.o.b. ${value.birthDate}`;
  }

}
