import React from 'react';
import { Card, CardContent, Typography, Button } from '@mui/material';

interface Props {
  item: any;
  onRemove: () => void;
}

const MenuItemCard: React.FC<Props> = ({ item, onRemove }) => {
  return (
    <Card sx={{ width: '100%' }}>
      <CardContent>
        <Typography variant="subtitle1">{item.name}</Typography>
        <Typography variant="body2">Price: ${item.price.toFixed(2)}</Typography>
        {item.availableModifiers?.map((mod: any, index: number) => (
          <Typography key={index} variant="caption">
            Modifier: {mod.name} (+${mod.priceAdjustment.toFixed(2)})
          </Typography>
        ))}
        <Button size="small" color="error" onClick={onRemove}>Remove</Button>
      </CardContent>
    </Card>
  );
};

export default MenuItemCard;