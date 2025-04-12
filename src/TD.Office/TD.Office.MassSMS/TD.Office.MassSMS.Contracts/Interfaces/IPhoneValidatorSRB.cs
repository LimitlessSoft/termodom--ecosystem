namespace TD.Office.MassSMS.Contracts.Interfaces;

public interface IPhoneValidatorSRB
{
	string Format(string number);
	bool IsValid(string number);
}
