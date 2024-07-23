using System.Security.Cryptography;

public class PasswordHasher
{
	private const int SaltSize = 16;
	private const int HashSize = 32;
	private const int Iterations = 100000;

	public string HashPassword(string password)
	{
		byte[] salt = new byte[SaltSize];
		using (var rng = RandomNumberGenerator.Create())
		{
			rng.GetBytes(salt);
		}

		using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
		{
			byte[] hash = pbkdf2.GetBytes(HashSize);

			byte[] hashBytes = new byte[SaltSize + HashSize];
			Array.Copy(salt, 0, hashBytes, 0, SaltSize);
			Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

			return Convert.ToBase64String(hashBytes);
		}
	}

	public bool VerifyPassword(string password, string hashedPassword)
	{
		byte[] hashBytes = Convert.FromBase64String(hashedPassword);
		if (hashBytes.Length < SaltSize + HashSize)
		{
			return false;
		}

		byte[] salt = new byte[SaltSize];
		Array.Copy(hashBytes, 0, salt, 0, SaltSize);

		byte[] storedHash = new byte[HashSize];
		Array.Copy(hashBytes, SaltSize, storedHash, 0, HashSize);

		using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
		{
			byte[] newHash = pbkdf2.GetBytes(HashSize);

			return ConstantTimeEquals(newHash, storedHash);
		}
	}

	private bool ConstantTimeEquals(byte[] a, byte[] b)
	{
		if (a == null || b == null || a.Length != b.Length)
		{
			return false;
		}

		int diff = 0;
		for (int i = 0; i < a.Length; i++)
		{
			diff |= (a[i] ^ b[i]);
		}
		return diff == 0;
	}
}

