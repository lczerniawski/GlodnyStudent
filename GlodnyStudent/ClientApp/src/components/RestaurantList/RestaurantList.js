import React, { Component } from 'react'
import ListItem from './ListItem';
import './RestaurantList.css'
import Filters from './Filters';
import Sort from './Sort';
/* import {host} from '../../config' */

export class RestaurantList extends Component {

  constructor(props) {
    super(props);
    this.handleInputChange = this.handleInputChange.bind(this);
    this.getData = this.getData.bind(this);
    this.state = {
      distance:5,// przykladowe dane statyczne ("polowa" najwyzej wsrod cen)
      price:55,// przykladowe dane statyczne ("polowa" najwyzej wsrod cen)
      cuisine:'',
      highestPrice: 99,
      sort:'priceGrowingly',
      restaurations: [ // przykladowe dane statyczne , dane z serwera beda pobierane po 20 i po kliknieciu  "dalej" beda doladowyawane
        /* {id:"0",name: "Piękna restauracja1", cuisine: "Włoska", address: "Jana Pawła2 21/37",reviewsCount:"69", image: '',highestPrice:99,distance:11},
        {id:"1",name: "Piękna restauracja2", cuisine: "Polska", address: "Jana Pawła2 21/37",reviewsCount:"169", image: '',highestPrice:50,distance:100},
        {id:"2",name: "Piękna restauracja3", cuisine: "Azjatycka", address: "Jana Pawła2 21/37",reviewsCount:"169", image: '',highestPrice:25,distance:13},
        {id:"3",name: "Piękna restauracja4", cuisine: "Amerykańska", address: "Jana Pawła2 21/37",reviewsCount:"69", image: '',highestPrice:100,distance:12}    */
      ],
      cuisines: ['Polska','Włoska','Azjatycka','Amerykańska'] // przykladowe dane statyczne
    };
  }

componentDidMount(){
  /* console.log(`Address: ${this.props.address}`); */
  this.sendData();
}


sendData() {
 /*  const address = `${host}/api/Restaurants/${this.props.address}`; */

 const address = `api/Restaurants/${this.props.address}`;
  fetch(address)
  .then(res => res.json())
  .then(
    (result) => { /* console.log(`Dane:${result[0].name}`);
 */      this.setState({
        restaurations: this.state.restaurations.concat(result)
      });
    },
    (error) => {
     /*  this.setState({
        isLoaded: true,
        error
      }); */
    }
  )
    }


    getData(event){
      console.log("eloo");
      const address = `api/Restaurants/?address=${this.state.address}&distance=${this.state.distance}
      &highestPrice=${this.state.distance}&cuisine=${this.state.cuisine}`;
      fetch(address)
  .then(res => res.json())
  .then(
    (result) => { /* console.log(`Dane:${result[0].name}`);
 */      this.setState({
        restaurations: this.state.restaurations.concat(result)
      });
    },
    (error) => {
     /*  this.setState({
        isLoaded: true,
        error
      }); */
    }
  )
    }



  handleInputChange(event) {
    const target = event.target;
    const value = target.value;
    const name = target.name;

    this.setState({
      [name]: value
    });
  }

  render() {
    const list = this.state.restaurations.map((restauration)=><ListItem key={restauration.id} name={restauration.name} 
                  address={restauration.address}  reviewsCount={restauration.reviewsCount} image={restauration.image}/>)
    return (    
      <div id="restaurantListContainer">
        <Filters onSetFilter={this.getData} cuisines={this.state.cuisines} distance={this.state.distance} price={this.state.price} highestPrice={this.state.highestPrice} onSetFilter={this.handleInputChange} />
        <Sort sort={this.state.sort} restaurations={this.state.restaurations} onSetSort={this.handleInputChange} />
        <ul id="restaurantList">{list}</ul>
      </div>
    )
  }
}
