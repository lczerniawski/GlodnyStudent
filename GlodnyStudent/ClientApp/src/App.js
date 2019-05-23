import React, { Component } from 'react';
 import { Route } from 'react-router'; 
import { Layout } from './components/Layout';
import { Home } from './components/MainPage/Home';
import {RestaurantList} from './components/RestaurantList/RestaurantList';
import RestaurantPage from './components/RestaurantPage/RestaurantPage';
import Registration from './components/Registration/Registration'

export default class App extends Component {

  render () {
    return (
      <Layout>
        <Route exact path='/'  component={Home} />
        <Route path='/ListaRestauracji' component={RestaurantList} />
        <Route path='/Restauracja' component={RestaurantPage} />
        <Route path="/Rejestracja" exact component={Registration} />
      </Layout>
    );
  }
}
