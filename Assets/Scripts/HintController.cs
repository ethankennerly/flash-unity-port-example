namespace Finegamedesign.Utils
{
	public sealed class HintController
	{
		public Hint model = new Hint();
		public HintView view;
		private ButtonController buttonController = new ButtonController();

		public void Setup()
		{
			buttonController.view.Listen(view.exitButton);
			int index;
			for (index = 0; index < DataUtil.Length(view.productButtons); index++)
			{
				var owner = view.productButtons[index];
				buttonController.view.Listen(owner);
				var count = SceneNodeView.GetChild(owner, "CountText");
				var price = SceneNodeView.GetChild(owner, "PriceText");
				TextView.SetText(count, model.GetCountText(index));
				TextView.SetText(price, model.GetPriceText(index));
			}
			for (index = 0; index < DataUtil.Length(view.storeButtons); index++)
			{
				var owner = view.storeButtons[index];
				buttonController.view.Listen(owner);
			}
		}

		public void Update()
		{
			buttonController.Update();
			if (buttonController.isAnyNow)
			{
				if (view.exitButton == buttonController.view.target)
				{
					model.Close();
				}
				else
				{
					int index;
					for (index = 0; index < DataUtil.Length(view.productButtons); index++)
					{
						var owner = view.productButtons[index];
						if (owner == buttonController.view.target)
						{
							model.Purchase(index);
						}
					}
					for (index = 0; index < DataUtil.Length(view.storeButtons); index++)
					{
						var owner = view.storeButtons[index];
						if (owner == buttonController.view.target)
						{
							model.Store();
						}
					}
				}
			}
			AnimationView.SetState(view.store, model.state);
		}
	}
}
