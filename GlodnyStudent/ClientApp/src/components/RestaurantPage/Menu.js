import React, { Component } from 'react';
import './Menu.css';


export default class Menu extends Component {


    constructor(props){
        super(props);
        this.state={
            id:null,
            name:"",
            price:null,
            disabledSubmit:true
        }
        this.inputValidate = this.inputValidate.bind(this);
    }


      inputValidate(event){
        
            let disable = true;
            let value = event.target.value;
            const name = event.target.name;

            if(name === "price"){
                disable = (!this.state.name && event.target.value);
            } else if(name === "name"){
                disable = !(this.state.price && value);
            }
             
            
            this.setState({
                disabledSubmit: disable,
                [name]:value
              });
      }


    render() {

        const menuList = this.props.menu.map(row=><li className="wow fadeIn" data-wow-duration="1s" key={row.id}>
            <span className="name">{row.name}</span> 
            <span className="price">{row.price}</span>
            {this.props.ownerLogIn&&sessionStorage.getItem("id")?
            <button className="buttonDelete" value={row.id}  onClick={(e)=>this.props.deleteMenuItem(e,`Menu/${row.id}`)}><span>Usuń</span></button>:""}       
        </li>);

            const addItemForm = this.props.ownerLogIn&&sessionStorage.getItem("id")?
            <div className="addMenu">
                    <h3>Dodaj danie do menu</h3>
                    <form>
                        <div className="label-form">
                            <label for="nameMenu">Danie:</label>
                            <input id="nameMenu" className="inputStyle" type="text" name="name" onChange={this.inputValidate} />
                        </div>
                        <div className="label-form">
                            <label for="priceMenu">Cena (zł):</label>
                            <input id="priceMenu" className="inputStyle" type="number" name="price" onChange={this.inputValidate} />
                        </div>                        
                        <input className="buttonAccept" type="submit" disabled={this.state.disabledSubmit} onClick={(e)=>this.props.addMenuItem(e,"menu",{name:this.state.name,price:this.state.price,restaurantId:this.props.restaurantId},"Menu")} value="Dodaj"/>
                    </form> 
                </div>:null;


        return (
            <div className="menu">
                <h3>Menu</h3>
                <ul>
                    {menuList}
                </ul>
                    {addItemForm}     
              </div>
        )
    }
}
