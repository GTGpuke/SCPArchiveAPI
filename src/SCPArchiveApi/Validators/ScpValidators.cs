// Validators : Utilisés pour valider les données reçues dans les DTOs avant de les traiter dans l'API.
// Permet d'assurer la cohérence et la sécurité des données côté serveur.

using FluentValidation;
using SCPArchiveApi.DTOs;

namespace SCPArchiveApi.Validators;

/// <summary>
/// Validateur pour le DTO principal d'un article SCP.
/// Vérifie le format du numéro, la présence des champs obligatoires, la validité de la classe, etc.
/// </summary>
public class ScpEntryDtoValidator : AbstractValidator<ScpEntryDto>
{
    public ScpEntryDtoValidator()
    {
        // Le numéro doit être non vide et respecter le format SCP-XXX
        RuleFor(x => x.ItemNumber)
            .NotEmpty()
            .Matches(@"^SCP-\d+$")
            .WithMessage("Le numéro d'article doit être au format 'SCP-XXX'");

        // Le titre est obligatoire et limité à 200 caractères
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);

        // La classe d'objet doit être parmi la liste autorisée
        RuleFor(x => x.ObjectClass)
            .NotEmpty()
            .Must(BeValidObjectClass)
            .WithMessage("La classe d'objet doit être Safe, Euclid, Keter, Thaumiel, ou Neutralized");

        // Le contenu doit être présent et validé par un validateur dédié
        RuleFor(x => x.Content)
            .NotNull()
            .SetValidator(new ScpContentDtoValidator());

        // Maximum 20 tags
        RuleFor(x => x.Tags)
            .NotNull()
            .Must(x => x.Count <= 20)
            .WithMessage("Un maximum de 20 tags est autorisé");
    }

    private bool BeValidObjectClass(string objectClass)
    {
        var validClasses = new[] { "Safe", "Euclid", "Keter", "Thaumiel", "Neutralized" };
        return validClasses.Contains(objectClass);
    }
}

/// <summary>
/// Validateur pour le contenu principal d'un article SCP.
/// </summary>
public class ScpContentDtoValidator : AbstractValidator<ScpContentDto>
{
    public ScpContentDtoValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(10000);

        RuleFor(x => x.Containment)
            .NotEmpty()
            .MaximumLength(5000);

        RuleForEach(x => x.Addenda)
            .SetValidator(new ScpAddendumDtoValidator());
    }
}

/// <summary>
/// Validateur pour un addendum d'article SCP.
/// </summary>
public class ScpAddendumDtoValidator : AbstractValidator<ScpAddendumDto>
{
    public ScpAddendumDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Content)
            .NotEmpty()
            .MaximumLength(5000);
    }
}
