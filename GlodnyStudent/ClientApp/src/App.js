import React, { Component } from 'react';
import { Route } from 'react-router'; 
import { Layout } from './components/Layout';
import { Home } from './components/MainPage/Home';
import {RestaurantList} from './components/RestaurantList/RestaurantList';
import RestaurantPage from './components/RestaurantPage/RestaurantPage';
import Registration from './components/Registration/Registration'
import AddRestaurant from './components/AddRestaurant/AddRestaurant';
import ResetPassword from './components/ForgetPassword/ResetPassword';
import ChangePassword from './components/ChangePassword/ChangePassword';
import NotFound from './components/NotFound/NotFound';


export default class App extends Component {

  render () {
    return (
      <Layout>
        <Route exact path='/'  component={Home} />
        <Route path='/ListaRestauracji' component={RestaurantList} />
        <Route path='/Restauracja' component={RestaurantPage} />
        <Route path="/Rejestracja" exact component={Registration} />
        <Route path='/DodajRestauracje' exact component={AddRestaurant}/>
        <Route path='/ResetHasła' component={ResetPassword}/>
        <Route path='/ZmianaHasła' component={ChangePassword}/>
        <Route path='/NieZnaleziono' component={NotFound}/>
      </Layout>
    );
  }
}
