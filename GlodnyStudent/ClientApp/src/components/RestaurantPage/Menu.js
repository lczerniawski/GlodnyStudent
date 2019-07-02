import React, { Component } from 'react';


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

        const menuList = this.props.menu.map(row=><li key={row.id}>
            <span>{row.name}</span> 
            <span>{row.price}</span>
            {this.props.ownerLogIn&&sessionStorage.getItem("id")?
            <button value={row.id}  onClick={(e)=>this.props.deleteMenuItem(e,`Menu/${row.id}`)}><span>Usuń</span></button>:""}       
        </li>);

            const addItemForm = this.props.ownerLogIn&&sessionStorage.getItem("id")?
            <div>
                    <h3>Dodaj danie do menu</h3>
                    {this.props.responseMessageMenuAddItem}
                    {this.props.responseMessageRemoveMenuItem}
                    <form>
                        <div>
                            <label>Danie:
                                <input id="nameMenu" type="text" name="name" onChange={this.inputValidate} />
                            </label>
                        </div>
                        <div>
                            <label>Cena (zł):
                                <input id="priceMenu" type="number" name="price" onChange={this.inputValidate} />
                            </label>
                        </div>                        
                        <input type="submit" disabled={this.state.disabledSubmit} onClick={(e)=>this.props.addMenuItem(e,{name:this.state.name,price:this.state.price,restaurantId:this.props.restaurantId},"Menu")} value="Dodaj"/>
                    </form> 
                </div>:null;


        return (
            <div>
                <h3>Menu</h3>
                <ul>
                    {menuList}
                </ul>
                    {addItemForm}     
              </div>
        )
    }
}
