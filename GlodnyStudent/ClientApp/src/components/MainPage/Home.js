import React, { Component } from 'react';
import Search from './Search';

export class Home extends Component {
/*   static displayName = Home.name; */
 
  
  render () {
    return (
      <div>
        <Search isMain="true" onAddressInput={this.props.onAddressInput} />
      </div>
    );
  }
}
