﻿namespace LimeTech_Components.Server.Areas.Admin.Controllers
{
    using AutoMapper;
    using Azure.Core;
    using LimeTech_Components.Server.DTOs;
    using LimeTech_Components.Server.Services.Components;
    using LimeTech_Components.Server.Services.Components.Models;
    using Microsoft.AspNetCore.Mvc;
    using static LimeTech_Components.Server.Constants.DataConstants;

    [ApiController]
    [Route("api/components")]
    public class ComponentsController : AdminController
    {
        private readonly IComponentService _componentService;
        private readonly IMapper _mapper;

        public ComponentsController(IComponentService componentService, IMapper mapper)
        {
            _componentService = componentService;
            _mapper = mapper;
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddComponentAsync([FromBody] AddComponentRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {

                var componentServiceModel = new ComponentServiceModel
                {
                    Name = request.Name,
                    Producer = request.Producer,
                    TypeOfProduct = request.TypeOfProduct,
                    ImageUrl = request.ImageUrl,
                    Price = request.Price,
                    PurchasedCount = request.PurchasedCount,
                    ProductionYear = request.ProductionYear,
                    PowerUsage = request.PowerUsage,
                    Status = request.Status,
                    StockCount = request.StockCount,
                    IsPublic = request.IsPublic,
                };

                await _componentService.AddComponentAsync(componentServiceModel);

                return StatusCode(201, "Component added successfully.");
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> EditComponentAsync(int id, [FromBody] EditComponentRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var componentServiceModel = new ComponentServiceModel
                {
                    Id = id,
                    Name = request.Name,
                    Producer = request.Producer,
                    TypeOfProduct = request.TypeOfProduct,
                    ImageUrl = request.ImageUrl,
                    Price = request.Price,
                    PurchasedCount = request.PurchasedCount,
                    ProductionYear = request.ProductionYear,
                    PowerUsage = request.PowerUsage,
                    Status = request.Status,
                    StockCount = request.StockCount,
                    IsPublic = request.IsPublic,
                };

                var result = await _componentService.EditComponentAsync(componentServiceModel);

                if (!result)
                {
                    return NotFound($"Component with ID {id} not found.");
                }

                return Ok("Component updated successfully.");
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        [HttpPatch("{id}/visibility")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChangeComponentVisibility(int id, [FromBody] bool isPublic)
        {
            try
            {
                var result = await _componentService.ChangeComponentVisibilityAsync(id, isPublic);

                if (!result)
                {
                    return NotFound($"Component with ID {id} not found.");
                }

                return Ok("Component visibility updated successfully.");
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }




        //[HttpGet]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> GetComponents(
        //[FromQuery] int currentPage = 1,
        //[FromQuery] int componentsPerPage = ComponentConstants.ComponentsPerPage)
        //{
        //    if (currentPage < 1 || componentsPerPage < 1)
        //    {
        //        return BadRequest("Current page and components per page must be greater than zero.");
        //    }

        //    try
        //    {
        //        var components = await _componentService.GetComponentsAsync(
        //            currentPage: currentPage,
        //            componentsPerPage: componentsPerPage,
        //            publicOnly: false);

        //        return Ok(components);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception
        //        return StatusCode(500, "An error occurred while retrieving components.");
        //    }
        //}



    }
}
