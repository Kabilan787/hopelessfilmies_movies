﻿using HopelessFilmies.Domain.Interfaces.IContact;
using HopelessFilmiesMVC.Data;
using HopelessFilmiesMVC.Models;
using Microsoft.AspNetCore.Mvc;

public class ContactController : Controller
{
    private readonly IContactService _contactService;

    public ContactController(IContactService contactService)
    {
        _contactService = contactService;
    }

    // GET: /Contact
    [HttpGet]
    public IActionResult Index(string? message)
    {
        ViewBag.Message = message;
        return View(); // Returns Views/Contact/Index.cshtml
    }

    // POST: /Contact
    [HttpPost]
    public async Task<IActionResult> Submit(string name, string email, string message)
    {
        if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(message))
        {
            //var contact = new ContactForm
            //{
            //    Name = name,
            //    Email = email,
            //    Message = message
            //};

            //_context.ContactForms.Add(contact);
            //_context.SaveChanges();

            await _contactService.AddContactAsync(name, email, message);

            return Ok(); // Respond with 200 OK for success
        }

        return BadRequest("Invalid input"); // Respond with 400 Bad Request for failure
    }



}
