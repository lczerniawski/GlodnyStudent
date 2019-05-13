import React, { Component } from 'react';
 import { Route } from 'react-router'; 
import { Layout } from './components/Layout';
import { Home } from './components/MainPage/Home';
import {RestaurantList} from './components/RestaurantList/RestaurantList';
/* import { FetchData } from './components/FetchData'; */
/* import { Counter } from './components/Counter'; */

export default class App extends Component {
  static displayName = App.name;

  constructor(props){
    super(props);
    this.state={ address :""}
    this.setAddress = this.setAddress.bind(this);
  } 

  setAddress(address){
    this.setState({address:address});
  }


  render () {
    return (
      <Layout>
       {/*  <Route exact path='/' component={Home} />
        <Route path='/counter' component={Counter} />
        <Route path='/fetch-data' component={FetchData} /> */}
        <Route exact path='/'  render={(props) => <Home {...props} onAddressInput={this.setAddress}  />} />
        <Route  path='/RestaurantList'  render={(props) => <RestaurantList  {...props} address={this.state.address} />} />
      </Layout>
    );
  }
}
