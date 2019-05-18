import React, { Component } from 'react';
 import { Route } from 'react-router'; 
import { Layout } from './components/Layout';
import { Home } from './components/MainPage/Home';
import {RestaurantList} from './components/RestaurantList/RestaurantList';
import RestaurantPage from './components/RestaurantPage/RestaurantPage';

export default class App extends Component {
  static displayName = App.name;

  constructor(props){
    super(props);
    this.state={ 
      address :"",
      id:""}
    this.setAddress = this.setAddress.bind(this);
    this.setId = this.setId.bind(this);
  } 

  setAddress(address){
    this.setState({address:address});
  }
  setId(id){
    this.setState({id:id});
  }


  render () {
    return (
      <Layout>
        <Route exact path='/'  render={(props) => <Home {...props} onAddressInput={this.setAddress}  />} />
        <Route  path='/ListaRestauracji'  render={(props) => <RestaurantList  {...props}  sendIdForRestaurantPage={this.setId}  address={this.state.address} />} />
        <Route  path='/Restauracja'  render={(props) => <RestaurantPage  {...props} id={this.state.id}  />} />
      </Layout>
    );
  }
}
