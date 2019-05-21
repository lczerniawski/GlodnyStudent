import React, { Component } from 'react';
 import { Route } from 'react-router'; 
import { Layout } from './components/Layout';
import { Home } from './components/MainPage/Home';
import {RestaurantList} from './components/RestaurantList/RestaurantList';
import RestaurantPage from './components/RestaurantPage/RestaurantPage';
import Registration from './components/Registration/Registration'

export default class App extends Component {

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
        <Route exact path='/'  render={(props) => <Home {...props}  onAddressInput={this.setAddress}  />} />
        <Route  path='/ListaRestauracji' exact render={(props) => <RestaurantList  {...props}  sendIdForRestaurantPage={this.setId}  address={this.state.address} />} />
        <Route  path='/Restauracja'  render={(props) => <RestaurantPage  {...props} id={this.state.id}  />} />
        <Route path="/Rejestracja" exact component={Registration} />
      </Layout>
    );
  }
}
