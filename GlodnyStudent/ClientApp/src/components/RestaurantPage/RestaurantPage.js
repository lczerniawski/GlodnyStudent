import React, { Component } from 'react';
import './RestaurantPage.css';
import ReviewsCreator from './ReviewsCreator';
import Rateing from './Rateing';


export default class RestaurantPage extends Component {


constructor(props){
    super(props);
    this.state={
        name:"",
        address:"",
        menu:[],
        gallery:[],
        reviews:[],
        rate:"",
        newReview:'',
        //addToRate:0
    };
    this.handleInputChange = this.handleInputChange.bind(this);
    this.sendReview =this.sendReview.bind(this);
    this.sendRate = this.sendRate.bind(this);
}
  
    componentDidMount(){
        this.getDataById();
      }
      
      getDataById(){
        const address = `api/Restaurants/${this.props.id}`;
        fetch(address).then((response) => {
          if (response.ok) {
            return response.json();
          } else {
            throw new Error('Restauration not found');
          }
        })
        .then((result) => {
          this.setState({
            error:false,
            name: result.name,
            address:result.address,
            menu:result.menu,
            gallery:result.gallery,
            reviews:result.reviews,
          });
        })
        .catch((error) => {
          console.log(error);
          this.setState({
            error
          }); 
        });
      }

 
      sendRate(event) {


         const adr = (event.target.val == -1)? 'https://localhost:44375/api/Restaurants/2/DownVote':'https://localhost:44375/api/Restaurants/2/UpVote' ;
        event.preventDefault(adr);
          fetch('', {
          method: 'POST',
          headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
          },
          body: JSON.stringify({
            id: this.props.id
          })
        }).then((res) => res.json())
        .then((data) =>  console.log(data))
        .catch((err)=>console.log(err))  


        
      }  


      /* sendReview(event) {
         
        event.preventDefault();
          fetch('', {
          method: 'POST',
          headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
          },
          body: JSON.stringify({
            id: this.props.id,// bedzie zmienione jak powstanie logowanie i konta uzytkownikow
          })
        })  
      } */


      
      handleInputChange(event) {
        const target = event.target;
        const value = target.value;
        const name = target.name;
    
        this.setState({
          [name]: value
        });
        //console.log(`nowa ocena${this.state.addToRate}`);
        //console.log(`nowa opinia${this.state.newReview}`);
      }



  render() {
  const menuList = this.state.menu.map(row=><li key={row.id} className="menuRow"><span>{row.name}</span><span>{row.price}</span></li>);
  const reviewsList = this.state.reviews.map(row=><li key={row.id} className="menuRow"><span>{row.description}</span><span>{row.addTime}</span></li>);
    return (
      <div>
        <div >TU BEDZIE NAGŁÓWKOWY OBRAZEK</div>
        <div>{this.state.name}</div>
        <section>
            <div>MAPA</div>
            <div>
                <h3>Znajdź nas na mapie!</h3>
                <p>{this.state.address}</p>
                <Rateing rate={this.state.rate} onRate={this.sendRate}/>
                <button>Zobacz na mapach google</button>
            </div>
        </section>
            Tu będzie galleria
        <section>
        <section>
            Tu będzie menu
        </section>    
            <ul>
                {menuList}
            </ul>
        </section>
        <section>
            <ReviewsCreator onReviewInput={this.handleInputChange} onSendReview={this.sendReview}/>
            <ul>
            {reviewsList}
            </ul> 
        </section>
      </div>
    )
  }
}
