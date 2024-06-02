using Agrigate.Api.Core;
using Agrigate.Api.Features.Authentication.Models;
using Agrigate.Core;
using Agrigate.Domain.DTOs;
using Agrigate.Domain.Messages.Authentication;
using Akka.Actor;
using Akka.Hosting;
using Microsoft.AspNetCore.Mvc;

public class AuthenticationController : AgrigateController
{
    public AuthenticationController(IRequiredActor<ApiSupervisor> supervisor) 
        : base(supervisor)
    {
    }

    /// <summary>
    /// Generates a new public / private key pair
    /// </summary>
    /// <returns></returns>
    // [HttpPost("Key")]
    // public IActionResult GenerateKeyPair()
    // {
    //     return Created(true);
    // }

    /// <summary>
    /// Returns the public RSA Key
    /// </summary>
    /// <returns></returns>
    // [HttpGet("Key")]
    // public IActionResult GetPublicKey()
    // {
    //     try
    //     {
    //         return Success(true);
    //     }
    //     // catch (Exception ex)
    //     catch (Exception)
    //     {
    //         // Logger.Error(ex, "Error generating public key: {Error}", ex.Message);
    //         return Failure("Error generating public key");
    //     }
    // }

    /// <summary>
    /// Attempts to register a new user
    /// </summary>
    /// <param name="registration">The user details to register</param>
    /// <returns></returns>
    [HttpPost("Register")]
    public async Task<IActionResult> Register(UserDTO registration)
    {
        try 
        {
            var result = await ApiSupervisor.Ask(
                new RegisterUser(registration.Username, registration.Password),
                Constants.MaxActorWaitTime
            );

            if (result is Exception exception)
                throw exception;

            return Success(result);
        }
        catch (Exception ex)
        {
            return Failure($"Unable to register user: {ex.Message}");
        }
    }

    /// <summary>
    /// Attempts to generates an auth token for the provided user
    /// </summary>
    /// <returns></returns>
    [HttpPost("Token")]
    public async Task<IActionResult> GenerateUserToken(UserDTO credentials)
    {
        try
        {
            var result = await ApiSupervisor.Ask(
                new GenerateToken(credentials.Username, credentials.Password),
                Constants.MaxActorWaitTime
            );

            if (result is Exception exception)
                throw exception;

            return Success(result);
        }
        catch(Unauthorized)
        {
            return Unauthorized();
        }
        catch (Exception ex)
        {
            return Failure($"Unable to generate token: {ex.Message}");
        }
    }
}