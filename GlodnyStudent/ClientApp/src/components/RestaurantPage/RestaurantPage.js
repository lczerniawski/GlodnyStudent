import React, { Component } from 'react';
import './RestaurantPage.css';
import ReviewsCreator from './ReviewsCreator';


export default class RestaurantPage extends Component {


constructor(props){
    super(props);
    this.state={
        restaurtion:{headerImage:"",name:"Przykładowa nazwa restauracji",address:"Jana Pawła 2 21/37",galleryImage:[],
        menu:[["Danie1", "2137zł"],["Danie2", "911zł"],["Danie3", "666zł"]],
        reviews:[["Tomke","Sztos","11-09-2001"],["Jan Paweł 2","Polecam i to jeszcze jak","02-04-2005"],["Łukaszke97","może być","01-01-2019"]],},
        newReview:''
    };
    this.handlenNewReview = this.handleNewReview.bind(this);
    this.sendReview =this.sendReview.bind(this);
}



    // Ustalić szczegoly komunikacji
    /*componentDidMount(){
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
            restaurations: result
          });
        })
        .catch((error) => {
          console.log(error);
          this.setState({
            error
          }); 
        });
      }*/


      sendReview(event) {
        event.preventDefault();
         /* fetch('', {
          method: 'POST',
          headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
          },
          body: JSON.stringify({
            nick: "Tymczasowy nick",// bedzie zmienione jak powstanie logowanie i konta uzytkownikow
            review: this.state.newReview,
          })
        })  */
      }


      
      handleNewReview(review){
        this.setState({newReview:review});
      } 




  render() {
    const { restaurtion} = this.state;
  const menuList = restaurtion.menu.map((row,i)=><li key={i} className="menuRow">{row.map((rowItem,i)=><span key={i}>{rowItem}</span>)}</li>);
  const reviewsList = restaurtion.reviews.map((row,i)=><li key={i} className="rewiew">{row.map((rowItem,i)=><span key={i}>{rowItem}</span>)}</li>);
    return (
      <div>
        <div >TU BEDZIE NAGŁÓWKOWY OBRAZEK</div>
        <div>{restaurtion.name}</div>
        <section>
            <div >MAPA</div>
            <div>
                <h3>Znajdź nas na mapie!</h3>
                <p>{restaurtion.address}</p>
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
            <ReviewsCreator onReviewInput={this.handleNewReview} onSendReview={this.sendReview}/>
            <ul>
            {reviewsList}
            </ul> 
        </section>
      </div>
    )
  }
}
