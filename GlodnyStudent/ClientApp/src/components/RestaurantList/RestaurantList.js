import React, { Component } from 'react'
import ListItem from './ListItem';
import Filters from './Filters';
import Sort from './Sort';
import Search from  '../MainPage/Search';
import './RestaurantList.css';
import './Search.css'; 
/* import {host} from '../../config' */

export class RestaurantList extends Component {

  constructor(props) {
    super(props);
    this.handleInputChange = this.handleInputChange.bind(this);
    this.getDataByFilters = this.getDataByFilters.bind(this);
    this.addressUpdate = this.addressUpdate.bind(this);
    this.state = {
      location:window.location.href.slice(window.location.href.lastIndexOf("/")+1),
      distance:20,// tu wstawić jako poczatkowa wartosc wartosc highestDistance
      price:0,// tu wstawić jako poczatkowa wartosc wartosc highestPrice
      cuisine:'Wszystkie',
      cuisines: [/* 'Amerykańska','Polska','Włoska','Azjatycka' */], // tu wstawic liste wszystkich typow kuchni z serwera
      highestPrice:0,// tu wstawić maksymalna wartość dostarczona z serwera
      sort:'priceGrowingly',
      restaurations: [ // przykladowe dane statyczne , dane z serwera beda pobierane po 20 i po kliknieciu  "dalej" beda doladowyawane
        /* {id:"0",name: "Piękna restauracja1", cuisine: "Włoska", address: "Jana Pawła2 21/37",reviewsCount:"69", image: '',highestPrice:99,distance:11},
        {id:"1",name: "Piękna restauracja2", cuisine: "Polska", address: "Jana Pawła2 21/37",reviewsCount:"169", image: '',highestPrice:50,distance:100},
        {id:"2",name: "Piękna restauracja3", cuisine: "Azjatycka", address: "Jana Pawła2 21/37",reviewsCount:"169", image: '',highestPrice:25,distance:13},
        {id:"3",name: "Piękna restauracja4", cuisine: "Amerykańska", address: "Jana Pawła2 21/37",reviewsCount:"69", image: '',highestPrice:100,distance:12}    */
      ]
      
    };
  }

componentDidMount(){
  this.getDataByAddress();
  this.getCuisinesAndHighestPrice();
}

getDataByAddress(){
  const address = `api/Search/${this.state.location}`;
  fetch(address).then((response) => {
    if (response.ok) {
      return response.json();
    } else {
      throw new Error('Restauration not found');
    }
  })
  .then((result) => {
    this.setState({
      restaurations: result
    });
  })
  .catch((error) => {
    console.log(error);
  });
}


getCuisinesAndHighestPrice(firstTime=true){
  const address = `api/Search/Cuisines/${this.state.location}`;
  fetch(address).then((response) => {
    if (response.ok) {
      return response.json();
    } else {
      throw new Error('Cuisines not found');
    }
  })
  .then((result) => {
      const prevPrice = this.state.price>result.highestPrice?result.highestPrice:this.state.price;
      this.setState({
      error:false,
      cuisines:result.cuisinesList,
      highestPrice: result.highestPrice,
      price:firstTime?result.highestPrice:prevPrice
    });
  })
  .catch((error) => {
    console.log(error); 
  });
}


    getDataByFilters(){
      const address = `api/Search?address=${this.state.location}&distance=${this.state.distance}&highestPrice=${this.state.price}&cuisine=${this.state.cuisine}`;
      fetch(address).then((response) => {
        if (response.ok) {
          return response.json();
        } else {
          throw new Error('Restauration not found');
        }
      })
      .then((result) => {
        this.setState({
          restaurations: result
        });
      })
      .catch((error) => {
        console.log(error);
      });
      this.getCuisinesAndHighestPrice(false);
    }



  handleInputChange(event) {
    const target = event.target;
    const value = target.value;
    const name = target.name;

    this.setState({
      [name]: value
    });
  }

  addressUpdate(addr){
    this.setState({
      location:addr
  }, () => {
    this.getDataByFilters();
  });
  }

  

  render() {
    const { restaurations,cuisines,distance,price,highestPrice,sort} = this.state;
    let list 
    if (restaurations.length === 0) {
      list= <div className="notFound wow bounce" data-wow-duration="1s">
        <i className="fas fa-bug fa-4x"></i>
        <h2>Upsss... nic nie znaleziono</h2>
        <p>Nie znaleziono restauracji o podanych parametrach.</p>
        </div>; 
    } else {
       list = restaurations.map((restauration)=><ListItem key={restauration.id} name={restauration.name} address={restauration.address} rate={restauration.score}
        reviewsCount={restauration.reviewsCount} image={restauration.image} id={restauration.id} />);
    }   
    return (    
      <div className="restaurantListContainer">

        <div className="titlePage">
          <h2 className="wow fadeInLeft" data-wow-duration="1s">Lista restauracji</h2>
        </div>

        <div className="searchRestaurant">
          <Search onAddressUpdate={this.addressUpdate} isMain={false} address={this.state.location}/>
        </div>

        <div className="filersBar">
      <Filters  cuisines={cuisines} distance={distance} price={price} highestPrice={highestPrice} onSetFilter={this.handleInputChange} />
          <Sort sort={sort} restaurations={restaurations} onSetSort={this.handleInputChange} />
        </div>
        
        <div className="restaurantList">
          {list}
        </div>
      </div>
    )
  }
}
