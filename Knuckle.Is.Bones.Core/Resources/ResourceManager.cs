using Knuckle.Is.Bones.Core.Models.Game;
using Knuckle.Is.Bones.Core.Models.Shop;
using System.Reflection;
using System.Text.Json;

namespace Knuckle.Is.Bones.Core.Resources
{
	public static class ResourceManager
	{
		private static readonly Guid _coreID = new Guid("f06a2262-4c0b-4bca-bd1d-d13504e572bc");

		public static BaseBuilder<BoardDefinition> Boards = new BaseBuilder<BoardDefinition>("Resources.Core.Boards", Assembly.GetExecutingAssembly());
		public static BaseBuilder<OpponentDefinition> Opponents = new BaseBuilder<OpponentDefinition>("Resources.Core.Opponents", Assembly.GetExecutingAssembly());
		public static BaseBuilder<DiceDefinition> Dice = new BaseBuilder<DiceDefinition>("Resources.Core.Dice", Assembly.GetExecutingAssembly());
		public static BaseBuilder<ShopItemDefinition> Shop = new BaseBuilder<ShopItemDefinition>("Resources.Core.Shop", Assembly.GetExecutingAssembly());

		public static List<ResourceDefinition> LoadedResources { get; internal set; } = new List<ResourceDefinition>() {
			new ResourceDefinition(_coreID, "1.0.0", "Core", "Core Game Components")
		};

		public static void LoadResource(DirectoryInfo path)
		{
			FileInfo? definitionFile = null;
			foreach (var file in path.GetFiles())
			{
				if (file.Extension.ToUpper() == ".JSON")
				{
					definitionFile = file;
					break;
				}
			}
			if (definitionFile == null || definitionFile.Directory == null)
				throw new FileNotFoundException("No resource definition found!");
			var resourceDefinition = JsonSerializer.Deserialize<ResourceDefinition>(File.ReadAllText(definitionFile.FullName));
			if (resourceDefinition == null)
				throw new Exception("Resource definition is malformed!");
			resourceDefinition.Path = path.FullName;

			foreach (var folder in definitionFile.Directory.GetDirectories())
			{
				if (folder.Name.ToUpper() == "BOARDS")
					Boards.LoadExternalResources(folder.GetFiles().ToList());
				else if (folder.Name.ToUpper() == "OPPONENTS")
					Opponents.LoadExternalResources(folder.GetFiles().ToList());
				else if (folder.Name.ToUpper() == "DICE")
					Dice.LoadExternalResources(folder.GetFiles().ToList());
				else if (folder.Name.ToUpper() == "SHOP")
					Shop.LoadExternalResources(folder.GetFiles().ToList());
			}

			if (!LoadedResources.Any(x => x.ID == resourceDefinition.ID))
				LoadedResources.Add(resourceDefinition);
		}

		public static void UnloadExternalResources()
		{
			Boards.Reload();
			Opponents.Reload();
			Dice.Reload();
			Shop.Reload();

			LoadedResources.Clear();
			LoadedResources = new List<ResourceDefinition>() {
				new ResourceDefinition(_coreID, "1.0.0", "Core", "Core Game Components")
			};
		}

		public static void ReloadResources()
		{
			Boards.Reload();
			Opponents.Reload();
			Dice.Reload();
			Shop.Reload();

			foreach (var resource in LoadedResources)
				if (resource.ID != _coreID)
					LoadResource(new DirectoryInfo(resource.Path));
		}
	}
}