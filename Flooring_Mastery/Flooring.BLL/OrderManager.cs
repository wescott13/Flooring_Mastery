using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flooring.BLL.Helpers;
using Flooring.Data.Data;
using Flooring.Models;
using Flooring.Models.Interfaces;
using Flooring.Models.Responses;

namespace Flooring.BLL
{
    public class OrderManager
    {
        //CTOR injection 
        //1. create private field of the interface type
        //2. create a ctor to take an instance of the interface as a parameter
        //3. assign the parameter instance to the private field
        private IOrderRepository _orderRepository;
        private IStateTaxRepository _stateTaxRepository;
        private IProductsRepository _productsRepository;

        public OrderManager(IOrderRepository orderRepository, IStateTaxRepository stateTaxRepository, IProductsRepository productsRepository)
        {
            _orderRepository = orderRepository;
            _stateTaxRepository = stateTaxRepository;
            _productsRepository = productsRepository;
        }
        public OrderResponse AddOrder(Order order)
        {
            OrderResponse response = new OrderResponse();
            OrderSettings orderSettings = new OrderSettings();

            var addOrderRepository = _orderRepository;
            if (addOrderRepository == null)
            {
                response.Success = false;
                response.Message = $"{order} was not added.";
            }
            else
            {
                response.Success = true;
                response.Order = order;
                _orderRepository.SaveOrder(order); 
            }
            return response;
        }
        public OrderResponse EditOrder(Order order)
        {
            OrderResponse response = new OrderResponse();
            OrderSettings orderSettings = new OrderSettings();

            var editOrderRepository = _orderRepository;
            if (editOrderRepository == null)
            {
                response.Success = false;
                response.Message = $"{order} was not added.";
            }
            else
            {
                response.Success = true;
                response.Order = order;

                _orderRepository.EditOrder(order);
            }
            return response;
        }
        public OrderResponse RemoveOrder(Order order)
        {
            OrderResponse response = new OrderResponse();
            OrderSettings orderSettings = new OrderSettings();

            var removeOrderRepository = _orderRepository;
            if (removeOrderRepository == null)
            {
                response.Success = false;
                response.Message = $"{order} was not removed.";
            }
            else
            {
                response.Success = true;
                response.Order = order;
                _orderRepository.RemoveOrder(order);
            }
            return response;
        }
        public OrderDateLookupResponse LookupOrderDate(DateTime date)
        {
            OrderDateLookupResponse response = new OrderDateLookupResponse();

            response.Orders = _orderRepository.DisplayOrdersByDate(date);

            if (response.Orders == null || !response.Orders.Any())
            {
                response.Success = false;
                response.Message = $" No orders are found on {date}.";
            }
            else
            {
                response.Success = true;
            }
            return response;
        }
        public StateTaxesLookupResponse LookupState(string state)
        {
            StateTaxesLookupResponse response = new StateTaxesLookupResponse();
            response.stateTax = _stateTaxRepository.LoadState(state);

            if (response.stateTax == null)
            {
                response.Success = false;
                response.Message = $"{state} is not  a valid state.";
            }
            else
            {
                response.Success = true;
            }

            return response;
        }
        public List<string> GetAllStates()
        {
            var states = new List<string>();
            states = _stateTaxRepository.GetAllStateTax();

            return states;
        }
        public Dictionary<string, Products> GetAllProducts()
        {
            var productsDictionary = new Dictionary<string, Products>();
            productsDictionary = _productsRepository.GetAllProducts2();
            
            foreach(var product in productsDictionary)
            {
                _productsRepository.LoadProduct(product.Key);
            }
            return productsDictionary;
        }
        public ProductLookUpResponse LookupProduct(string product)
        {
            ProductLookUpResponse response = new ProductLookUpResponse();

            response.product = _productsRepository.LoadProduct(product);

            if (response.product == null)
            {
                response.Success = false;
                response.Message = $"{product} is not  a valid product.";
            }
            else
            {
                response.Success = true;
            }
            return response;
        }
    }
}
